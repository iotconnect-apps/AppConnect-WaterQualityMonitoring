import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { AuthService } from '../services/index';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import { Notification, NotificationService } from 'app/services';
import { AppConstant } from '../app.constants';
import { Router, ActivatedRoute } from '@angular/router'

/*
The JWT interceptor intercepts the incoming requests from the application/user and adds JWT token to the request's Authorization header, only if the user is logged in.
This JWT token in the request header is required to access the SECURE END API POINTS on the server 
*/

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private http: HttpClient,
    private _notificationService: NotificationService,
    private _appConstant: AppConstant,
    private authService: AuthService,
    private route: ActivatedRoute

  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // check if the current user is logged in
    // if the user making the request is logged in, he will have JWT token in it's local storage, which is set by Authorization Service during login process
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.access_token) {
      var res = request.url.split("/");
      var image_url_gh = request.url.includes("api/building/manage");
      var image_url_device = request.url.includes("api/device/manage");
      var image_url_entity = request.url.includes("api/entity/manage");
      if (image_url_gh || image_url_device || image_url_entity) {

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.access_token}`,
            //'Content-Type': 'multipart/form-data;',
            'company-id': currentUser.userDetail.companyId,
          }
        });

      } else {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.access_token}`,
            'Content-Type': 'application/json',
            'company-id': currentUser.userDetail.companyId,
          }
        });
      }
    }

    // handle any other requests which went unhandled
    return next.handle(request).catch(err => {
      // add error message
      let error = (err.error) ? ((err.error.Message) ? err.error.Message : this._appConstant.serverErrorMessage) : ((err.statusText) ? err.statusText : this._appConstant.serverErrorMessage);
      if (error == 'Unauthorized' && !this._notificationService.refreshTokenInProgress) {
        if (currentUser.userDetail.isAdmin) {
          this.authService.logout();
          this.router.navigate(['/admin']);
          error = "Your login session has expired.";
        } else {
          this._notificationService.refreshTokenInProgress = true;
          //Genrate params for token refreshing
          let params = {
            token: currentUser.refresh_token
          };
          // token refreshing request
          return this.http.post(this._notificationService.apiBaseUrl + "api/account/refreshtoken", params).flatMap(
            (response: any) => {
              this._notificationService.refreshTokenInProgress = false;
              //If reload successful update tokens
              if (response.isSuccess == true && response.data) {
                //Update tokens
                let currentUser = JSON.parse(localStorage.getItem('currentUser'));
                if (response.data) {
                  currentUser.access_token = response.data.access_token;
                  currentUser.refresh_token = response.data.refresh_token;
                  localStorage.setItem('currentUser', JSON.stringify(currentUser));
                }
                //Clone our fields request and try to resend it
                request = request.clone({
                  setHeaders: {
                    Authorization: `Bearer ${currentUser.access_token}`,
                    'Content-Type': 'application/json',
                    'company-id': currentUser.userDetail.companyId,
                  }
                });

                return next.handle(request).catch((err: any) => {
                  //Catch another error
                  error = (err.error) ? err.error.Message : (err.statusText) ? err.statusText : this._appConstant.serverErrorMessage;
                  return throwError(error);
                });

              } else {
                //Logout from account
                localStorage.removeItem('currentUser');
                this._notificationService.add(new Notification('error', this._appConstant.tokenInValidMessage));
                location.reload(true);
                return;
              }
            }
          );
        }
      }
      if (err.status === 401 && !currentUser) {
        // auto logout on unauthorized response
        localStorage.removeItem('currentUser');
        this._notificationService.add(new Notification('error', this._appConstant.unauthorizedMessage));
        location.reload(true);
        return;
      }

      return throwError(error);

    });

  }

}

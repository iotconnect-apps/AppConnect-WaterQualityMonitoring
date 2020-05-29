import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { ApiConfigService, NotificationService } from 'app/services';
@Injectable({
  providedIn: 'root'
})
export class SensorService {
  protected apiServer = ApiConfigService.settings.apiServer;
  cookieName = 'FM';
  constructor(private cookieService: CookieService,
    private httpClient: HttpClient,
    private _notificationService: NotificationService) {
    this._notificationService.apiBaseUrl = this.apiServer.baseUrl
  }

  checkkitCode(data) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/ValidateKit/' + data).map(response => {
      return response;
    });
  }

  addUpdateSensor(data) {
    const formData = new FormData();
    for (const key of Object.keys(data)) {
      const value = data[key];
      if (data[key])
        formData.append(key, value);
    }

    return this.httpClient.post<any>(this.apiServer.baseUrl + 'api/device/manage', formData).map(response => {
      return response;
    });
  }

  getsensorDetails(sensorGuid) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/' + sensorGuid).map(response => {
      return response;
    });
  }

  gettemplatelookup() {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/alltemplate').map(response => {
      return response;
    });

  }

  getBuildinglookup(parentEntityId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/entity/search/?parentEntityId=' + parentEntityId).map(response => {
      return response;
    });

  }

  getsensorlist(parameters) {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    var configHeader = {
      headers: {
        'Content-Type': 'application/json',
      }
    };

    const parameter = {
      params: {
        'pageNo': parameters.pageNumber + 1,
        'pageSize': parameters.pageSize,
        'searchText': parameters.searchText,
        'orderBy': parameters.sortBy
      },
      timestamp: Date.now()
    };
    var reqParameter = Object.assign(parameter, configHeader);

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/search', reqParameter).map(response => {
      return response;
    });
  }

  SensorchangeStatus(id, isActive) {
    let status = isActive == true ? false : true;
    return this.httpClient.post<any>(this.apiServer.baseUrl + 'api/device/updatestatus/' + id + '/' + status, {}).map(response => {
      return response;
    });
  }
}

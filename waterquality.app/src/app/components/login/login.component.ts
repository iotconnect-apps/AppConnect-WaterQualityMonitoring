import { Component, OnInit, ViewEncapsulation } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Router } from '@angular/router'
import { NgxSpinnerService } from 'ngx-spinner'
import { UserService, AdminAuthGuired  } from '../../services/index'
import { Notification, NotificationService } from 'app/services';


@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
	loginform: FormGroup;
	checkSubmitStatus = false;
	loginObject = {};
	loginStatus = false;
	constructor(
		private spinner: NgxSpinnerService,
		private router: Router,
		private _notificationService: NotificationService,
		public UserService : UserService,
		private authService: AdminAuthGuired
	) { }

	ngOnInit() {
		this.loginStatus = this.authService.isCheckLogin();
		if (this.loginStatus === true) {
		  this.router.navigate(['dashboard']);
		}
		this.createFormGroup();
		// logout the person when he opens the app for the first time
		//this.UserService.logout();
	}

	createFormGroup() {
		this.loginform = new FormGroup({
			username: new FormControl('',[Validators.required,Validators.email]),
			password: new FormControl('',[Validators.required]),
		});

		this.loginObject = {
			username: '',
			password: '',
		};
	}

	login() {
		this.checkSubmitStatus = true;
		if (this.loginform.status === "VALID" && this.checkSubmitStatus) {
			this.spinner.show();
			this.UserService.login(this.loginObject).subscribe(response => {
				this.spinner.hide();
				if (response.isSuccess === true && response.data.access_token) {
					this._notificationService.add(new Notification('success', 'Logged in successfully'));
					this.router.navigate(['/']);
				}
				else {
					this._notificationService.add(new Notification('error', response.message));
					this.router.navigate(['/login']);
				}
			}, error => {
				this.spinner.hide();
				this._notificationService.add(new Notification('error', error ));
			});
		}
	}

	forgotPassword($event) {
		$("#divLoginSection").hide();
		$("#divForgotPwdSection").show();
	}
	
	forgotPasswordCancel($event) {
		$("#divForgotPwdSection").hide();
		$("#divLoginSection").show();

	}
}

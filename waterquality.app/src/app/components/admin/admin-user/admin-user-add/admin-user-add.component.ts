import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { FormControl, FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms'
import { NgxSpinnerService } from 'ngx-spinner'
import { NotificationService, UserService, Notification } from '../../../../services';
import { CustomValidators } from '../../../../helpers/custom.validators';

@Component({
	selector: 'app-adminuser-add',
	templateUrl: './admin-user-add.component.html',
	styleUrls: ['./admin-user-add.component.css']
})
export class AdminUserAddComponent implements OnInit {
	public contactNoError:boolean=false;
	public mask = {
		guide: true,
		showMask: false,
		keepCharPositions: true,
		mask: ['(', /[0-9]/, /\d/, ')', '-', /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/]
	};

	fileToUpload: any = null;
	moduleName = "Add User";
	userObject = {};
	userGuid = '';
	isEdit = false;
	userForm: FormGroup;
	checkSubmitStatus = false;
	buttonname = 'Submit'

	constructor(
		private formBuilder: FormBuilder,
		private router: Router,
		private _notificationService: NotificationService,
		private activatedRoute: ActivatedRoute,
		private spinner: NgxSpinnerService,
		public userService: UserService,
			) {
		this.activatedRoute.params.subscribe(params => {
			if (params.userGuid != null) {
				this.getUserDetails(params.userGuid);
				this.userGuid = params.userGuid;
				this.moduleName = "Edit User";
				this.isEdit = true;
				this.buttonname = 'Update'
			} else {
				this.userObject = { firstName: '', lastName: '', email: '', contactNo: '', password: '' }
			}
		});
		this.createFormGroup();
	}

	ngOnInit() {

	}

  /**
   * Create Form
   * */
	createFormGroup() {

			this.userForm = this.formBuilder.group({
			    id:[''],
				firstName: ['', Validators.required],
				lastName: ['', Validators.required],
				email: ['', [Validators.required, Validators.email]],
				contactNo: ['', Validators.required],
				password: ['', [
					Validators.required, 
					Validators.pattern('(?!.* )(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z\d$@$!@#$%^&*].{7,20}')]
				],
        confirmPassword: ['', Validators.required],
				companyGuid:['']
			},{
          validators: [CustomValidators.checkPhoneValue('contactNo'), this.passwordConfirming]
			});
  }

  /**
   * Password and confirm password match validation
   * @param c
   */
  passwordConfirming(c: AbstractControl): { invalid: boolean } {

    if (c.get('password').value !== c.get('confirmPassword').value) {
      return { invalid: true };
    }
  }

  /**
   * Add/Update User
   * */
  manageUser() {
	this.checkSubmitStatus = true;
    let contactNo = this.userForm.value.contactNo.replace("(", "")
    let contactno = contactNo.replace(")", "")
    let finalcontactno = contactno.replace("-", "")
    if(finalcontactno.match(/^0+$/)){
      this.contactNoError=true;
      return
    } else {
      this.contactNoError=false;
    }
		this.userForm.get('id').setValue('00000000-0000-0000-0000-000000000000');
		if (this.userForm.status === "VALID") {
			if (this.isEdit) {
				this.userForm.registerControl("id", new FormControl(''));
				this.userForm.patchValue({"id" : this.userGuid});
			}
			this.spinner.show();
			 let currentUser = JSON.parse(localStorage.getItem('currentUser'));
			this.userForm.get('companyGuid').setValue(currentUser.userDetail.companyId);
      this.userForm.get('contactNo').setValue(contactno);
			this.userService.addadminUser(this.userForm.value).subscribe(response => {
				if (response.isSuccess === true) {
					this.spinner.hide();
					if (response.data.updatedBy != null) {
						this._notificationService.add(new Notification('success', "User has been updated successfully."));
					} else {
						this._notificationService.add(new Notification('success', "User has been added successfully."));
					}
					this.router.navigate(['/admin/users']);
				} else {
					this.spinner.hide();
					this._notificationService.add(new Notification('error', response.message));
				}
			})
		}
  }

  /**
   * Get user details by user guid
   * @param userGuid
   */
	getUserDetails(userGuid) {
		this.spinner.show();
		this.userService.getadminUserDetails(userGuid).subscribe(response => {
			this.spinner.hide();
			if (response.isSuccess === true) {
				this.userObject = response.data;
				this.userForm.controls.confirmPassword.setValue(response.data.password);
			}
		});
  }

  /**
   * Convert val to lower case
   * @param val
   */
	getdata(val) {
		return val = val.toLowerCase();
	}
}

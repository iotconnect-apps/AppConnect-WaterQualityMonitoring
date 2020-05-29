import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { NgxSpinnerService } from 'ngx-spinner'
import { DeviceService, NotificationService, LookupService } from 'app/services';
import { Notification } from 'app/services/notification/notification.service';
import { AppConstant } from "../../../app.constants";
import { Guid } from "guid-typescript";
import { Observable, forkJoin } from 'rxjs';



export interface DeviceTypeList {
	id: number;
	type: string;
}
export interface StatusList {
	id: boolean;
	status: string;
}
@Component({
	selector: 'app-generator-add',
	templateUrl: './generator-add.component.html',
	styleUrls: ['./generator-add.component.css']
})


export class GeneratorAddComponent implements OnInit {
	unique = false;
	currentUser: any;
	fileUrl: any;
	fileName = '';
	fileToUpload: any = null;
	status;
	moduleName = "Add Generator";
	parentDeviceObject: any = {};
	deviceObject = {};
	deviceGuid = '';
	parentDeviceGuid = '';
	isEdit = false;
	genraterForm: FormGroup;
	checkSubmitStatus = false;
	templateList = [];
	tagList = [];
	typeList=[];
	locationList=[];
	statusList: StatusList[] = [
		{
			id: true,
			status: 'Active'
		},
		{
			id: false,
			status: 'In-active'
		}

	];
	genraterObject: any = {};
	
	generatorGuid: any;
	constructor(
		private router: Router,
		private _notificationService: NotificationService,
		private activatedRoute: ActivatedRoute,
		private spinner: NgxSpinnerService,
		private deviceService: DeviceService,
		private lookupService: LookupService,
		public _appConstant: AppConstant
	) {
		this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
		this.activatedRoute.params.subscribe(params => {
			// set data for parent device
			if (params.generatorGuid != null) {
				this.getgenraterDetail(params.generatorGuid);
				this.generatorGuid = params.generatorGuid;
				this.moduleName = "Edit Generator";
				this.isEdit = true;
			}else {
				this.genraterObject = { locationGuid: '', typeGuid: '', name: '', templateGuid: '', uniqueId: '',kitcode:''}
			}
		});
	}

	// before view init
	ngOnInit() {
		this.createFormGroup();
		this.getLocationLookup();
		this.gettypeLookup();
		this.gettemplateLookup();
	}

	

	createFormGroup() {
		this.genraterForm = new FormGroup({
			imageFile:new FormControl(''),
			guid: new FormControl(''),
			companyGuid: new FormControl(null),
			name: new FormControl('', [Validators.required]),
			locationGuid: new FormControl('', [Validators.required]),
			typeGuid:new FormControl('', [Validators.required]),
			parentGensetGuid: new FormControl(''),
			uniqueId:new FormControl('', [Validators.required,Validators.pattern('^[A-Za-z0-9 ]+$')]),
			tag: new FormControl(''),
			note: new FormControl(''),
			kitcode: new FormControl('', [Validators.required]),
			isProvisioned: new FormControl(false),
			isActive: new FormControl(true),
			specification:new FormControl(''),
			description:new FormControl('')
		});
	}

	/**
	 * Get all the data related to parent device using forkjoin (Combine services)
	 * 
	 * @param deviceGuid 
	 * 
	 */
	getChildDeviceData(deviceGuid) {

		this.spinner.show();
		this.deviceService.getDeviceDetails(deviceGuid).subscribe(response => {
			if (response.isSuccess === true) {
				this.deviceObject = response.data;
			} else {
				this._notificationService.add(new Notification('error', response.message));
			}
		}, error => {
			this.spinner.hide();
			this._notificationService.add(new Notification('error', error));
		});
	}

	/**
	 * set parent device details
	 * @param response 
	 */
	setParentDeviceDetails(response) {
		if (response.isSuccess === true) {
			this.parentDeviceObject = response.data;
			//Get tags lookup once parent device data is fetched
			//this.getTagsLookup();
		} else {
			this._notificationService.add(new Notification('error', response.message));
		}

	}

	/**
	 * set template lookup
	 * only gateway supported template
	 *  @param response
	 */
	setTemplateLookup(response) {
		if (response.isSuccess === true) {
			this.templateList = response['data'];
		} else {
			this._notificationService.add(new Notification('error', response.message));
		}
	}

	/**
	 * Get tags lookup once parent device data is fetched
	 */
	getLocationLookup() {
		let currentUser = JSON.parse(localStorage.getItem('currentUser'));
			this.lookupService.getlocation(currentUser.userDetail.companyId).
				subscribe(response => {
					if (response.isSuccess === true) {
						this.locationList = response['data'];
					} else {
						this._notificationService.add(new Notification('error', response.message));
					}
				}, error => {
					this.spinner.hide();
					this._notificationService.add(new Notification('error', error));
				})

	}

	gettypeLookup() {
		//let currentUser = JSON.parse(localStorage.getItem('currentUser'));
			this.lookupService.gettypelookup().
				subscribe(response => {
					if (response.isSuccess === true) {
						this.typeList = response['data'];
					} else {
						this._notificationService.add(new Notification('error', response.message));
					}
				}, error => {
					this.spinner.hide();
					this._notificationService.add(new Notification('error', error));
				})

	}
	gettemplateLookup() {
		//let currentUser = JSON.parse(localStorage.getItem('currentUser'));
			this.lookupService.gettemplatelookup().
				subscribe(response => {
					if (response.isSuccess === true) {
						this.templateList = response['data'];
					} else {
						this._notificationService.add(new Notification('error', response.message));
					}
				}, error => {
					this.spinner.hide();
					this._notificationService.add(new Notification('error', error));
				})

	}
	
	log(obj) {
	}


	/**
	 * Find a value from the look up data
	 * 
	 * @param obj 
	 * 
	 * @param findByvalue 
	 * 
	 */
	getIndexByValue(obj, findByvalue) {
		let index = obj.findIndex(
			(tmpl) => { return (tmpl.value == findByvalue.toUpperCase()) }
		);
		if (index > -1) return obj[index].text;
		return;
	}


	/**
	 * Add device under gateway
	 * only gateway supported device
	 */
	
	addGenerater() {
		this.checkSubmitStatus = true;
		this.genraterForm.get('guid').setValue(null);
		if (this.genraterForm.status === "VALID") {
			this.deviceService.checkkitCode(this.genraterForm.value.kitcode).subscribe(response => {
				this.spinner.hide();
				if (this.fileToUpload) {
					this.genraterForm.get('imageFile').setValue(this.fileToUpload);
				  }
				if (response.isSuccess === true) {
					if (this.isEdit) {
						this.genraterForm.registerControl("guid", new FormControl(''));
						this.genraterForm.patchValue({"guid" : this.generatorGuid});
					}
					this.spinner.show();
					 let currentUser = JSON.parse(localStorage.getItem('currentUser'));
					this.genraterForm.get('parentGensetGuid').setValue(currentUser.userDetail.entityGuid);
					this.genraterForm.get('companyGuid').setValue(currentUser.userDetail.companyId);
					this.deviceService.addUpdateGenrator(this.genraterForm.value).subscribe(response => {
						if (response.isSuccess === true) {
							this.spinner.hide();
							if (response.data.updatedBy != null) {
								this._notificationService.add(new Notification('success', "Generator has been updated successfully."));
							} else {
								this._notificationService.add(new Notification('success', "Generator has been added successfully."));
							}
							this.router.navigate(['generators']);
						} else {
							this.spinner.hide();
							this._notificationService.add(new Notification('error', response.message));
						}
					})
				}
				else {
				  this._notificationService.add(new Notification('error', 'Kit not found'));
				}
			  }, error => {
				this.spinner.hide();
				this._notificationService.add(new Notification('error', error));
			  });
			
		}
	}
	getgenraterDetail(genraterGuid) {

		this.spinner.show();
		this.deviceService.getgenraterDetails(genraterGuid).subscribe(response => {
			if (response.isSuccess === true) {
				this.spinner.hide();
				this.genraterObject = response.data;
			} else {
				this._notificationService.add(new Notification('error', response.message));
			}
		}, error => {
			this.spinner.hide();
			this._notificationService.add(new Notification('error', error));
		});
	}
	getdata(val) {
		if(val){
			return val = val.toLowerCase();
		}
	}

	handleImageInput(event) {
		let files = event.target.files;
		if (files.length) {
		  let fileType = files.item(0).name.split('.');
		  let imagesTypes = ['jpeg', 'JPEG', 'jpg', 'JPG', 'png', 'PNG'];
		  if (imagesTypes.indexOf(fileType[fileType.length - 1]) !== -1) {
			this.fileName = files.item(0).name;
			this.fileToUpload = files.item(0);
      } else {
			this.fileToUpload = null;
			this.fileName = '';
		  }
		}
	
		if (event.target.files && event.target.files[0]) {
		  var reader = new FileReader();
		  reader.readAsDataURL(event.target.files[0]);
		  reader.onload = (innerEvent: any) => {
			this.fileUrl = innerEvent.target.result;
		  }
		}
	  }
}

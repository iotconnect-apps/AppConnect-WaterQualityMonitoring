import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { NgxSpinnerService } from 'ngx-spinner'
import { BuildingService } from '../../../services/building/building.service';
import { Notification, NotificationService } from '../../../services';

import { AppConstant, MessageAlertDataModel, DeleteAlertDataModel } from '../../../app.constants';
import { MatDialog } from '@angular/material';
import { MessageDialogComponent, DeleteDialogComponent } from '../..';

export interface DeviceTypeList {
  id: number;
  type: string;
}

@Component({
  selector: 'app-building-add',
  templateUrl: './building-add.component.html',
  styleUrls: ['./building-add.component.css']
})
export class BuildingAddComponent implements OnInit {

  @ViewChild('myFile', { static: false }) myFile: ElementRef;
  validstatus = false;
  handleImgInput = false;
  MessageAlertDataModel: MessageAlertDataModel;
  deleteAlertDataModel: DeleteAlertDataModel;
  currentImage: any;
  fileUrl: any;
  fileName = '';
  fileToUpload: any;
  hasImage = false;
  status;
  moduleName = "Add Building";
  buildingObject: any = {};
  buildingGuid = '';
  isEdit = false;
  buildingForm: FormGroup;
  checkSubmitStatus = false;
  countryList = [];
  stateList = [];
  mediaUrl: any;
  buttonname = 'Submit'
  constructor(
    private router: Router,
    private _notificationService: NotificationService,
    private activatedRoute: ActivatedRoute,
    private spinner: NgxSpinnerService,
    public buildingService: BuildingService,
    public _appConstant: AppConstant,
    public dialog: MatDialog,
  ) {
    this.createFormGroup();
    this.activatedRoute.params.subscribe(params => {
      if (params.buildingGuid != 'add') {
        this.isEdit = true;
        this.buildingGuid = params.buildingGuid;
        this.moduleName = "Edit Building";
        this.buttonname = 'Update'
        setTimeout(() => {
          this.getBuildingDetails(params.buildingGuid);
        }, 1500);
        this.getcountryList();
      } else {
        this.getcountryList();
        this.buildingObject = { name: '', zipcode: '', countryGuid: '', stateGuid: '', isActive: 'true', city: '', latitude: '', longitude: '' }
      }
    });
  }

  ngOnInit() {
    this.mediaUrl = this._notificationService.apiBaseUrl;

  }

  imageRemove() {
    this.myFile.nativeElement.value = "";
    if (this.buildingObject['image'] == this.currentImage) {
      this.buildingForm.get('imageFile').setValue('');
      if (!this.handleImgInput) {
        this.handleImgInput = false;
        this.deleteImgModel();
      }
      else {
        this.handleImgInput = false;
      }
    }
    else {
      if (this.currentImage) {
        this.spinner.hide();
        this.buildingObject['image'] = this.currentImage;
        this.fileToUpload = false;
        this.fileName = '';
        this.fileUrl = null;
      }
      else {
        this.spinner.hide();
        this.buildingObject['image'] = null;
        this.buildingForm.get('imageFile').setValue('');
        this.fileToUpload = false;
        this.fileName = '';
        this.fileUrl = null;
      }
    }
  }


  deleteImgModel() {
    this.deleteAlertDataModel = {
      title: "Delete Image",
      message: this._appConstant.msgConfirm.replace('modulename', "Building Image"),
      okButtonName: "Confirm",
      cancelButtonName: "Cancel",
    };
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '400px',
      height: 'auto',
      data: this.deleteAlertDataModel,
      disableClose: false
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deletebuildingImg();
      }
    });
  }

  deletebuildingImg() {
    this.spinner.show();
    this.buildingService.removeBuildingImage(this.buildingGuid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        this.currentImage = '';
        this.buildingObject['image'] = null;
        this.buildingForm.get('imageFile').setValue('');
        this._notificationService.add(new Notification('success', this._appConstant.msgDeleted.replace("modulename", "Building Image")));
      } else {
        this._notificationService.add(new Notification('error', response.message));
      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  createFormGroup() {
    this.buildingForm = new FormGroup({
      parentEntityGuid: new FormControl(''),
      countryGuid: new FormControl(null, [Validators.required]),
      stateGuid: new FormControl(null, [Validators.required]),
      city: new FormControl('', [Validators.required]),
      name: new FormControl('', [Validators.required]),
      zipcode: new FormControl('', [Validators.required, Validators.pattern('^[A-Z0-9 _]*$')]),
      description: new FormControl(''),
      address: new FormControl('', [Validators.required]),
      isActive: new FormControl('', [Validators.required]),
      guid: new FormControl(null),
      latitude: new FormControl('', [Validators.required, Validators.pattern('^(\\+|-)?(?:90(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\\.[0-9]{1,6})?))$')]),
      longitude: new FormControl('', [Validators.required, Validators.pattern('^(\\+|-)?(?:180(?:(?:\\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\\.[0-9]{1,6})?))$')]),
      imageFile: new FormControl(''),
    });
  }

  addBuilding() {
    this.checkSubmitStatus = true;

    if (this.isEdit) {
      this.buildingForm.get('guid').setValue(this.buildingGuid);
      this.buildingForm.get('isActive').setValue(this.buildingObject['isActive']);
    } else {
      this.buildingForm.get('isActive').setValue(true);
    }
    if (this.buildingForm.status === "VALID") {
      if (this.validstatus == true || !this.buildingForm.value.imageFile) {
        if (this.fileToUpload) {
          this.buildingForm.get('imageFile').setValue(this.fileToUpload);
        }
        this.spinner.show();
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.buildingForm.get('parentEntityGuid').setValue(currentUser.userDetail.entityGuid);

        this.buildingService.addBuilding(this.buildingForm.value).subscribe(response => {
          this.spinner.hide();
          if (response.isSuccess === true) {
            if (this.isEdit) {
              this._notificationService.add(new Notification('success', "Building has been updated successfully."));
            } else {
              this._notificationService.add(new Notification('success', "Building has been added successfully."));
            }
            this.router.navigate(['/buildings']);
          } else {
            this._notificationService.add(new Notification('error', response.message));
          }
        });
      } else {
        this.MessageAlertDataModel = {
          title: "Building Image",
          message: "Invalid Image Type.",
          message2: "Upload .jpg, .jpeg, .png Image Only.",
          okButtonName: "OK",
        };
        const dialogRef = this.dialog.open(MessageDialogComponent, {
          width: '400px',
          height: 'auto',
          data: this.MessageAlertDataModel,
          disableClose: false
        });
      }
    }
  }

  removeFile(type) {
    if (type === 'image') {
      this.fileUrl = '';
      //this.floor_image_Ref.nativeElement.value = '';
    }
  }

  handleImageInput(event) {
    this.handleImgInput = true;
    let files = event.target.files;
    var that = this;
    if (files.length) {
      let fileType = files.item(0).name.split('.');
      let imagesTypes = ['jpeg', 'JPEG', 'jpg', 'JPG', 'png', 'PNG'];
      if (imagesTypes.indexOf(fileType[fileType.length - 1]) !== -1) {
        this.validstatus = true;
        this.fileName = files.item(0).name;
        this.fileToUpload = files.item(0);
        if (event.target.files && event.target.files[0]) {
          var reader = new FileReader();
          reader.readAsDataURL(event.target.files[0]);
          reader.onload = (innerEvent: any) => {
            this.fileUrl = innerEvent.target.result;
            that.buildingObject.image = this.fileUrl;
          }
        }
      } else {
        this.imageRemove();
        this.MessageAlertDataModel = {
          title: "Building Image",
          message: "Invalid Image Type.",
          message2: "Upload .jpg, .jpeg, .png Image Only.",
          okButtonName: "OK",
        };
        const dialogRef = this.dialog.open(MessageDialogComponent, {
          width: '400px',
          height: 'auto',
          data: this.MessageAlertDataModel,
          disableClose: false
        });
      }
    }


  }

  getBuildingDetails(buildingGuid) {
    this.spinner.show();
    this.buildingService.getbuildingDetails(buildingGuid).subscribe(response => {
      if (response.isSuccess === true) {
        this.buildingObject = response.data;
        if (this.buildingObject.image) {
          this.buildingObject.image = this.mediaUrl + this.buildingObject.image;
          this.currentImage = this.buildingObject.image;
          this.hasImage = true;
        } else {
          this.hasImage = false;
        }
        this.buildingService.getcitylist(response.data.countryGuid).subscribe(response => {
          this.stateList = response.data;
          this.spinner.hide();
        });
      }
    });
  }

  getcountryList() {
    //if (!this.isEdit) {
      this.spinner.show();
    //}
    this.buildingService.getcountryList().subscribe(response => {
      
      if (!this.isEdit) {
        this.spinner.hide();
      }
      this.countryList = response.data;
    });
  }

  changeCountry(event) {
    this.buildingForm.controls['stateGuid'].setValue(null, { emitEvent: true })
    if (event) {
      let id = event.value;
      this.spinner.show();
      this.buildingService.getcitylist(id).subscribe(response => {
        this.spinner.hide();
        this.stateList = response.data;
      });
    }
  }

  getdata(val) {
    return val = val.toLowerCase();
  }

}

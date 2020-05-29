import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { NgxSpinnerService } from 'ngx-spinner'
import { AppConstant, MessageAlertDataModel } from "../../../app.constants";
import { MatDialog } from '@angular/material';
import { BuildingService } from '../../../services/building/building.service';
import { Notification, NotificationService, UserService, SensorService, LookupService } from '../../../services';
import { MessageDialogComponent } from '../..';

export interface StatusList {
  id: boolean;
  status: string;
}

@Component({
  selector: 'app-sensor-add',
  templateUrl: './sensor-add.component.html',
  styleUrls: ['./sensor-add.component.css']
})
export class SensorAddComponent implements OnInit {

  @ViewChild('myFile', { static: false }) myFile: ElementRef;
  selectWing: string = "No Wing";
  selectBuilding: string = "No Building";
  validstatus = false;
  MessageAlertDataModel: MessageAlertDataModel;
  parentEntityId = '';
  public mask = {
    guide: true,
    showMask: false,
    keepCharPositions: true,
    mask: ['(', /[0-9]/, /\d/, ')', '-', /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/]
  };
  fileUrl: any;
  fileName = '';
  fileToUpload: any = null;
  status;
  moduleName = "Add Sensor";
  userObject = {};
  userGuid = '';
  isEdit = false;
  sensorForm: FormGroup;
  checkSubmitStatus = false;
  buildingList = [];
  wingList = [];
  selectedBuilding = '';
  floorList = [];
  spaceList = [];
  roleList = [];
  cityList = [];
  templateList = [];
  buttonname = 'SUBMIT'
  arrystatus = [{ "name": "Active", "value": true }, { "name": "Inactive", "value": false }]
  timezoneList = [];
  enitityList = [];
  sensorGuid: any;
  companyId: any;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private _notificationService: NotificationService,
    private activatedRoute: ActivatedRoute,
    private spinner: NgxSpinnerService,
    public userService: UserService,
    public sensorService: SensorService,
    public lookupService: LookupService,
    public dialog: MatDialog,
    public _appConstant: AppConstant

  ) {
    this.activatedRoute.params.subscribe(params => {
      if (params.sensorGuid != null) {
        this.getsensorDetails(params.sensorGuid);
        this.sensorGuid = params.sensorGuid;
        this.moduleName = "Edit Sensor";
        this.isEdit = true;
        this.buttonname = 'UPDATE'
      } else {
        this.userObject = { firstName: '', lastName: '', email: '', contactNo: '', password: '' }
      }
    });
    this.createFormGroup();
  }

  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.companyId = currentUser.userDetail.companyId
    this.gettemplateLookup();
    this.getBuildingLookup(this.companyId);
  }

  createFormGroup() {

    this.sensorForm = this.formBuilder.group({
      imageFile: [''],
      //templateGuid: ['', Validators.required],
      buildingGuid: ['', Validators.required],
      entityGuid: ['', Validators.required],
      name: ['', Validators.required],
      kitcode: ['', [Validators.required, Validators.pattern('^[A-Za-z0-9 ]+$')]],
      uniqueId: ['', [Validators.required, Validators.pattern('^[A-Za-z0-9]+$')]],
      //companyGuid:[''],
      specification: [''],
      description: ['']
    });
  }

  addSensor() {
    this.checkSubmitStatus = true;
    if (this.sensorForm.status === "VALID") {
      if (this.validstatus == true || this.sensorForm.value.imageFile == '') {
        this.sensorService.checkkitCode(this.sensorForm.value.kitcode).subscribe(response => {
          this.spinner.hide();
          if (this.fileToUpload) {
            this.sensorForm.get('imageFile').setValue(this.fileToUpload);
          }
          if (response.isSuccess === true) {
            if (this.isEdit) {
              //this.sensorForm.registerControl("guid", new FormControl(''));
              //this.sensorForm.patchValue({"guid" : this.sensorGuid});
            }
            this.spinner.show();
            let currentUser = JSON.parse(localStorage.getItem('currentUser'));
            this.sensorService.addUpdateSensor(this.sensorForm.value).subscribe(response => {
              if (response.isSuccess === true) {
                this.spinner.hide();
                this._notificationService.add(new Notification('success', "Sensor has been added successfully."));
                this.router.navigate(['sensorkits']);
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
      } else {
        this.MessageAlertDataModel = {
          title: "Sensor Image",
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

  getsensorDetails(sensorGuid) {
    this.spinner.show();
    this.sensorService.getsensorDetails(sensorGuid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        console.log("datss", response.data)
        //this.userObject = response.data;
        //this.fileUrl = this.deviceObject['image'];
      }
    });
  }

  getdata(val) {
    return val = val.toLowerCase();
  }

  gettemplateLookup() {
    //let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.sensorService.gettemplatelookup().
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

  getBuildingLookup(companyId) {
    this.lookupService.getBuildingLookup(companyId).
      subscribe(response => {
        if (response.isSuccess === true) {

          this.buildingList = response.data;
          this.buildingList = this.buildingList.filter(word => word.isActive == true);
          if (this.buildingList.length > 0) {
            this.selectBuilding = "Select Building";
          }
          else {
            this.selectBuilding = "No Building";
          }
        } else {
          this._notificationService.add(new Notification('error', response.message));
        }
      }, error => {
        this.spinner.hide();
        this._notificationService.add(new Notification('error', error));
      })

  }

  getwings(buildingId) {
    this.lookupService.getWingLookup(buildingId).
      subscribe(response => {
        if (response.isSuccess === true) {

          this.wingList = response.data;
          if (this.wingList.length <= 0) {
            this.selectWing = "No Wing";
          }
          else {
            this.selectWing = "Select Wing";
          }
        } else {
          this._notificationService.add(new Notification('error', response.message));
        }
      }, error => {
        this.spinner.hide();
        this._notificationService.add(new Notification('error', error));
      })
  }

  handleImageInput(event) {
    let files = event.target.files;
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
          }
        }
      } else {
        this.imageRemove();
        this.MessageAlertDataModel = {
          title: "Sensor Image",
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

  /**
 * Remove image
 * */
  imageRemove() {
    this.myFile.nativeElement.value = "";
    this.sensorForm.get('imageFile').setValue('');
    this.fileUrl = null;
    this.fileToUpload = false;
    this.fileName = '';
  }
}

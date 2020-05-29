import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { BuildingService } from '../../../services/building/building.service';
import { AppConstant, DeleteAlertDataModel } from '../../../app.constants';
import { Notification, NotificationService } from '../../../services';
import { DeleteDialogComponent } from '../..';

@Component({
  selector: 'app-building-list',
  templateUrl: './building-list.component.html',
  styleUrls: ['./building-list.component.css']
})
export class BuildingListComponent implements OnInit {

  moduleName = "Buildings";
  deleteAlertDataModel: DeleteAlertDataModel;
  isSearch = false;
  buildingList: any = [];
  searchParameters = {
    parentEntityGuid: '',
    pageNumber: -1,
    pageSize: -1,
    searchText: "",
    sortBy: "name asc"
  };

  constructor(
    private spinner: NgxSpinnerService,
    private router: Router,
    public dialog: MatDialog,
    public buildingService: BuildingService,
    public _appConstant: AppConstant,
    private _notificationService: NotificationService
  ) { }

  ngOnInit() {
    this.getbuildingList();
  }

  getbuildingList() {
    this.buildingList = [];
    this.spinner.show();
    this.buildingService.getBuilding(this.searchParameters).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        if (response.data.count) {
          response.data.items.forEach(element => {
            
            let entityDetailsObj = {};
            if (element.attributeList) {
              element.attributeList.forEach(attr => {
                entityDetailsObj[attr.key] = attr.value;
              })
            }
            let obj = {
              totalAlerts: element.totalAlerts,
              totalSubEntities: element.totalSubEntities,
              totalDevices: element.totalDevices,
              entityDetails: entityDetailsObj,
              guid: element.guid ,
              name: element.name,
              type: element.type,
              description: element.description,
              address: element.address,
              address2: element.address2,
              city: element.city,
              zipcode: element.zipcode,
              isActive: element.isActive
            };
            this.buildingList.push(obj);
          });
        } else {
          this.buildingList = [];
        }
      }
      else {
        this._notificationService.add(new Notification('error', response.message));
        this.buildingList = [];
      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  searchTextCallback(filterText) {
    this.searchParameters.searchText = filterText;
    this.searchParameters.pageNumber = 0;
    this.getbuildingList();
    this.isSearch = true;
  }

  deleteModel(userModel: any) {
    this.deleteAlertDataModel = {
      title: "Delete Building",
      message: this._appConstant.msgConfirm.replace('modulename', "Building"),
      okButtonName: "Yes",
      cancelButtonName: "No",
    };
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '400px',
      height: 'auto',
      data: this.deleteAlertDataModel,
      disableClose: false
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deletebuilding(userModel.guid);
      }
    });
  }

  deletebuilding(guid) {
    this.spinner.show();
    this.buildingService.deleteBuilding(guid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        this._notificationService.add(new Notification('success', this._appConstant.msgDeleted.replace("modulename", "Building")));
        this.getbuildingList();

      }
      else {
        this._notificationService.add(new Notification('error', response.message));
      }

    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  activeInactiveBuilding(id: string, isActive: boolean, name: string) {
    var status = isActive == false ? this._appConstant.activeStatus : this._appConstant.inactiveStatus;
    var mapObj = {
      statusname: status,
      fieldname: name,
      modulename: "Building"
    };
    this.deleteAlertDataModel = {
      title: "Status",
      message: this._appConstant.msgStatusConfirm.replace(/statusname|fieldname/gi, function (matched) {
        return mapObj[matched];
      }),
      okButtonName: "Yes",
      cancelButtonName: "No",
    };
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '400px',
      height: 'auto',
      data: this.deleteAlertDataModel,
      disableClose: false
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.changeBuildingStatus(id, isActive);

      }
    });
  }
    changeBuildingStatus(id, isActive) {
      this.spinner.show();
      this.buildingService.changeStatus(id, isActive).subscribe(response => {
        this.spinner.hide();
        if (response.isSuccess === true) {
          this._notificationService.add(new Notification('success', this._appConstant.msgStatusChange.replace("modulename", "Building")));
          this.getbuildingList();
        }
        else {
          this._notificationService.add(new Notification('error', response.message));
        }

      }, error => {
        this.spinner.hide();
        this._notificationService.add(new Notification('error', error));
      });
    }
}

import * as moment from 'moment-timezone'
import { Component, OnInit, ViewChild } from '@angular/core'
import { NgxSpinnerService } from 'ngx-spinner'
import { Router } from '@angular/router'
import { AppConstant } from "../../app.constants";
import { MatDialog} from '@angular/material'
import { BuildingService } from '../../services/building/building.service';
import { Notification,NotificationService, AlertsService } from '../../services';
import { SlickCarouselComponent } from 'ngx-slick-carousel';

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.css'],
})

export class DashboardComponent implements OnInit {

  slideConfig = {
    'slidesToShow': 3,
    'slidesToScroll': 1,
    'arrows': true,
    'margin': '30px',
    'centerMode': false
  };

  @ViewChild('homeCarousel', { static: true }) homeCarousel: SlickCarouselComponent;

  nextSlide() {
    this.homeCarousel.slickNext();
  }

  prevSlide() {
    this.homeCarousel.slickPrev();
  }
  alertsearchParameters = {
    pageNo: 0,
    pageSize: 10,
    orderBy: 'eventDate desc',
    deviceGuid: '',
    entityGuid:''
  };
  searchParameters = {
    parentEntityGuid: '',
    pageNumber: -1,
    pageSize: -1,
    searchText: "",
    sortBy: "name asc"
  };
  buildingList = [];
  alerts = [];
  lat = 25.2048;
  lng = 55.2708;
	isShowLeftMenu = true;
	mapview = true;
  currentUser = JSON.parse(localStorage.getItem('currentUser'));
	constructor(
		private router: Router,
		private spinner: NgxSpinnerService,
    public _appConstant: AppConstant,
    private _notificationService: NotificationService,
		public dialog: MatDialog,
    private buildingService: BuildingService,
    private alertsService: AlertsService,

	) { }

	ngOnInit() {
    this.getbuildingList();
    this.getAlertList();
  }

  getAlertList() {
    this.spinner.show();
    this.alertsService.getAlerts(this.alertsearchParameters).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        if (response.data.count) {
          this.alerts = response.data.items;
        }
      }
      else {
        this.alerts = [];
        this._notificationService.add(new Notification('error', response.message));
      }
    }, error => {
        this.spinner.hide();
        this.alerts = [];
      this._notificationService.add(new Notification('error', error));
    });
  }

  getbuildingList() {
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
              guid: element.guid,
              name: element.name,
              type: element.type,
              description: element.description,
              address: element.address,
              address2: element.address2,
              city: element.city,
              zipcode: element.zipcode,
              latitude: element.latitude,
              longitude: element.longitude
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

  getLocalDate(lDate) {
    var utcDate = moment.utc(lDate, 'YYYY-MM-DDTHH:mm:ss.SSS');
    // Get the local version of that date
    var localDate = moment(utcDate).local();
    let res = moment(localDate).format('MMM DD, YYYY hh:mm:ss A');
    return res;

  }
}

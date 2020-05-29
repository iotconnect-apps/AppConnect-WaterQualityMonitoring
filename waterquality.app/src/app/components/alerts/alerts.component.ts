import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner'
import { NotificationService, Notification, AlertsService } from '../../services';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment-timezone';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.css']
})
export class AlertsComponent implements OnInit {
  alerts = [];
  displayedColumns: string[] = ['message', 'parentEntityName', 'entityName', 'deviceName', 'eventDate', 'severity'];
  pageSizeOptions: number[] = [5, 10, 25, 100];
  order = true;
  isSearch = false;
  reverse = false;
  orderBy = 'name';
  searchParameters = {
    pageNo: 0,
    pageSize: 10,
    orderBy: 'eventDate desc',
    deviceGuid: '',
    entityGuid: ''
  };
  totalRecords = 0;
  constructor(
    private spinner: NgxSpinnerService,
    public _service: AlertsService,
    private _notificationService: NotificationService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.deviceGuid) {
        this.searchParameters.deviceGuid = params.deviceGuid;
      }
      if (params.entityGuid) {
        this.searchParameters.entityGuid = params.entityGuid;
      }
    });
    this.getAlertList();
  }

  getAlertList() {
    this.spinner.show();
    this._service.getAlerts(this.searchParameters).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true && response.data.items) {
        this.alerts = response.data.items;
        this.totalRecords = response.data.count;
      }
      else {
        this.alerts = [];
        this._notificationService.add(new Notification('error', response.message));
        this.totalRecords = 0;
      }
    }, error => {
        this.spinner.hide();
      this.alerts = [];
      this.totalRecords = 0;
      this._notificationService.add(new Notification('error', error));
    });
  }


  setOrder(sort: any) {
    if (!sort.active || sort.direction === '') {
      return;
    }
    this.searchParameters.orderBy = sort.active + ' ' + sort.direction;
    this.getAlertList();
  }


  onPageSizeChangeCallback(pageSize) {
    this.searchParameters.pageSize = pageSize;
    this.searchParameters.pageNo = 1;
    this.isSearch = true;
    this.getAlertList();
  }

  ChangePaginationAsPageChange(pagechangeresponse) {
    this.searchParameters.pageNo = pagechangeresponse.pageIndex;
    this.searchParameters.pageSize = pagechangeresponse.pageSize;
    this.isSearch = true;
    this.getAlertList();
  }

  getLocalDate(lDate) {
    var utcDate = moment.utc(lDate, 'YYYY-MM-DDTHH:mm:ss.SSS');
    // Get the local version of that date
    var localDate = moment(utcDate).local();
    let res = moment(localDate).format('MMM DD, YYYY hh:mm:ss A');
    return res;

  }
}

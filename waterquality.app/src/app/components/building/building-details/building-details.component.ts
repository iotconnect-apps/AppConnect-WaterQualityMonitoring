import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TelemetryPopupComponent } from '../../common/telemetry-popup/telemetry-popup.component';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BuildingService } from '../../../services/building/building.service';
import { AppConstant, DeleteAlertDataModel, MessageAlertDataModel } from '../../../app.constants';
import { DashboardService, NotificationService, Notification, DeviceService, AlertsService } from '../../../services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DeleteDialogComponent, MessageDialogComponent } from '../..';
import { Location } from '@angular/common';
import { StompRService } from '@stomp/ng2-stompjs'
import { Message } from '@stomp/stompjs'
import { Subscription } from 'rxjs'
import { Observable, forkJoin } from 'rxjs';
import * as moment from 'moment-timezone'
import * as _ from 'lodash'

@Component({
  selector: 'app-building-details',
  templateUrl: './building-details.component.html',
  styleUrls: ['./building-details.component.css'],
  providers: [StompRService]
})
export class BuildingDetailsComponent implements OnInit {

  @ViewChild('myFile', { static: false }) myFile: ElementRef;

  handleImgInput = false;
  isOpenFilterGraph: boolean = false;
  isOpenFilterGraph2: boolean = false;
  columnChart2 = {
    chartType: "ColumnChart",
    dataTable: [],
    options: {
      title: "",
      vAxis: {
        title: "",
        titleTextStyle: {
          bold: true
        },
        viewWindow: {
          min: 0
        }
      },
      hAxis: {
        title: "",
        titleTextStyle: {
          bold: true
        },
      },
      legend: { position: "none", alignment: "start" },
      bar: { groupWidth: "25%" },
      colors: ['#5496d0'],
      height: "400",
      width: "100%",
      series: [

      ]
    }
  };
  columnChart3:any = {};
  selectedIndex: any;
  checkvalue = true;
  checkvaluequaility = true;
  currentImage: any;
  isConnected = false;
  subscription: Subscription;
  messages: Observable<Message>;
  cpId = '';
  subscribed;
  stompConfiguration = {
    url: '',
    headers: {
      login: '',
      passcode: '',
      host: ''
    },
    heartbeat_in: 0,
    heartbeat_out: 2000,
    reconnect_delay: 5000,
    debug: true
  }
  chartColors: any = {
    red: 'rgb(255, 99, 132)',
    orange: 'rgb(255, 159, 64)',
    yellow: 'rgb(255, 205, 86)',
    green: 'rgb(75, 192, 192)',
    blue: 'rgb(54, 162, 235)',
    purple: 'rgb(153, 102, 255)',
    grey: 'rgb(201, 203, 207)',
    cerise: 'rgb(255,0,255)',
    popati: 'rgb(0,255,0)',
    dark: 'rgb(5, 86, 98)',
    solid: 'rgb(98, 86, 98)',
    tenwik: 'rgb(13, 108, 179)',
    redmek: 'rgb(143, 25, 85)',
    yerows: 'rgb(249, 43, 120)',
    redies: 'rgb(225, 208, 62)',
    orangeies: 'rgb(225, 5, 187)',
    yellowies: 'rgb(74, 210, 80)',
    greenies: 'rgb(74, 210, 165)',
    blueies: 'rgb(128, 96, 7)',
    purpleies: 'rgb(8, 170, 196)',
    greyies: 'rgb(122, 35, 196)',
    ceriseies: 'rgb(243, 35, 196)',
    popatiies: 'rgb(243, 35, 35)',
    darkies: 'rgb(87, 17, 35)',
    solidies: 'rgb(87, 71, 35)',

  };
  datasets: any[] = [
    {
      label: 'Dataset 1 (linear interpolation)',
      backgroundColor: 'rgb(153, 102, 255)',
      borderColor: 'rgb(153, 102, 255)',
      fill: false,
      lineTension: 0,
      borderDash: [8, 4],
      data: []
    }
  ];
  optionsdata: any = {
    type: 'line',
    scales: {

      xAxes: [{
        type: 'realtime',
        time: {
          stepSize: 10
        },
        realtime: {
          duration: 90000,
          refresh: 1000,
          delay: 2000,
          //onRefresh: '',

          // delay: 2000

        }

      }],
      yAxes: [{
        scaleLabel: {
          display: true,
          labelString: 'value'
        }
      }]

    },
    tooltips: {
      mode: 'nearest',
      intersect: false
    },
    hover: {
      mode: 'nearest',
      intersect: false
    }

  };
  validstatus = false;
  MesageAlertDataModel: MessageAlertDataModel;
  buildingGuid: any;
  buildingObj = {};
  isEdit = false;
  public respondShow: boolean = false;
  checkSubmitStatus = false;
  fileName: any;
  fileToUpload: any;
  fileUrl: any;
  buttonname = 'Submit';
  wingModuleName = "";
  wingForm: FormGroup;
  sensorList = [];
  wingList = [];
  attname = [];
  alerts = [];
  wingObject: any = {};
  wingGuid: any;
  mediaUrl: any;
  qualityParamType: any = 'w';
  deleteAlertDataModel: DeleteAlertDataModel;
  searchParameters = {
    parentEntityGuid: '',
    pageNumber: 0,
    pageSize: -1,
    searchText: '',
    sortBy: 'name asc'
  };
  alertsearchParameters = {
    pageNo: 0,
    pageSize: 5,
    orderBy: 'eventDate desc',
    deviceGuid: '',
    entityGuid: ''
  };
  wings = [
    { value: 'wing-a', viewValue: 'Wing A' },
    { value: 'wing-b', viewValue: 'Wing B' },
    { value: 'wing-c', viewValue: 'Wing C' }
  ];

  public canvasWidth = 250
  public needleValue = 0
  public centralLabel = ''
  public name = 'WQI'
  public bottomLabel = ' '
  public options = {
    hasNeedle: false,
    needleColor: 'gray',
    needleUpdateSpeed: 1000,
    arcColors: ['', ''],
    arcDelimiters: [50],
    rangeLabel: ['0', '100'],
    needleStartValue: 0,
  }

  WidgetData = {};
  smallWidgetData: any[] = [
    {
      smallWidgetValue: '25',
      smallWidgetTitle: 'Temperature'
    },
    {
      smallWidgetValue: '4.9',
      smallWidgetTitle: 'pH'
    },
    {
      smallWidgetValue: '20',
      smallWidgetTitle: 'Sodium'
    },
    {
      smallWidgetValue: '30',
      smallWidgetTitle: 'TDS'
    },
    {
      smallWidgetValue: '2.0',
      smallWidgetTitle: 'Turbidity'
    },
    {
      smallWidgetValue: '200',
      smallWidgetTitle: 'Conductivity'
    },
    {
      smallWidgetValue: '220',
      smallWidgetTitle: 'Chloride'
    },
    {
      smallWidgetValue: '40',
      smallWidgetTitle: 'Nitrate'
    },
    {
      smallWidgetValue: '5',
      smallWidgetTitle: 'Oxygen'
    },
  ];

  public columnChart = {
    chartType: 'ColumnChart',
    dataTable: [
      ['Day', 'Prediction', 'Actual Consumption'],
      ['Mon', 700, 1200],
      ['Tue', 300, 600],
      ['Web', 400, 500],
      ['Thu', 500, 1000],
      ['Fri', 600, 1100],
      ['Sat', 800, 1000],
      ['Sun', 100, 400]
    ],
    options: {
      bar: { groupWidth: "25%" },
      colors: ['#5496d0'],
      title: '',
      height: 250,
      // width:'100%',
      hAxis: {
        title: 'Day',
        gridlines: {
          count: 5
        },
      },
      vAxis: {
        title: 'Gallons',
        gridlines: {
          count: 1
        },
      }
    }
  };
  sensId: any;
  senName: any;
  winguid: any;
  sensguid: any;
  textlabel: any;
  buildingname: any;
  ishow: boolean;
  sensorId: any;
  sensguids:any;
  deviceIsConnected = false;

  constructor(
    private stompService: StompRService,
    private deviceService: DeviceService,
    private activatedRoute: ActivatedRoute,
    private spinner: NgxSpinnerService,
    public service: BuildingService,
    public dialog: MatDialog,
    public _appConstant: AppConstant,
    public dashboardService: DashboardService,
    private _notificationService: NotificationService,
    public location: Location,
    private alertsService: AlertsService,
    private buildingService: BuildingService,
  ) {
    this.createFormGroup();
    this.activatedRoute.params.subscribe(params => {
      if (params.buildingGuid) {
        this.buildingGuid = params.buildingGuid
      }

    })
  }

  ngOnInit() {
    this.ishow = false;
    this.onTabChange();
    this.getWingList(this.buildingGuid);
    this.getBuildingDetails(this.buildingGuid);
    this.mediaUrl = this._notificationService.apiBaseUrl;
    this.getStompConfig();
  }

  imageRemove() {
    this.myFile.nativeElement.value = "";
    if (this.wingObject['image'] == this.currentImage) {
      this.wingForm.get('imageFile').setValue('');
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
        this.wingObject['image'] = this.currentImage;
        this.fileToUpload = false;
        this.fileName = '';
        this.fileUrl = null;
      }
      else {
        this.spinner.hide();
        this.wingObject['image'] = null;
        this.wingForm.get('imageFile').setValue('');
        this.fileToUpload = false;
        this.fileName = '';
        this.fileUrl = null;
      }
    }
  }

  deletewingImg() {
    this.spinner.show();
    this.buildingService.removeBuildingImage(this.wingObject.guid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        this.currentImage = '';
        this.wingObject['image'] = null;
        this.wingForm.get('imageFile').setValue(null);
        this.fileToUpload = false;
        this.fileName = '';
        this.getWingList(this.buildingGuid);
        this._notificationService.add(new Notification('success', this._appConstant.msgDeleted.replace("modulename", "Wing Image")));

      } else {
        this._notificationService.add(new Notification('error', response.message));
      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  deleteImgModel() {
    this.deleteAlertDataModel = {
      title: "Delete Image",
      message: this._appConstant.msgConfirm.replace('modulename', "Wing Image"),
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
        this.deletewingImg();
      }
    });
  }

  onTabChangedata(event) {
    let type = this.qualityParamType;

    this.textlabel = event.tab.textLabel;
    this.getStaticsGraph(type, event.tab.textLabel);
  }

  changeGraphCunssumer(event) {
    this.checkvalue = false;
    let type = 'W';
    if (event.value === 'week') {
      type = 'W';
    } else if (event.value === 'month') {
      type = 'M';
    }
    this.getCunsummerGraph(type, this.sensId);
  }

  getCunsummerGraph(type, sensguid) {
    this.service.getCunsummerGraph(type, sensguid).subscribe(response => {
      let data = [];
      var hAxis= {
        title: "",
        slantedText:true,
        slantedTextAngle:45,
        titleTextStyle: {
          bold: true
        }
      }
      if (response.isSuccess === true) {
        if (response.data.length) {
          data.push(["Time", ""])
          response.data.forEach(element => {
            data.push([element.name, parseFloat(element.waterConsumption)])
          });
        }
        
        this.columnChart2 = {
          chartType: "ColumnChart",
          dataTable: data,
          options: {
            title: "",
            vAxis: {
              title: "",
              titleTextStyle: {
                bold: true
              },
              viewWindow: {
                min: 0
              }
            },
            hAxis: hAxis,
            legend: { position: "none", alignment: "start" },
            bar: { groupWidth: "25%" },
            colors: ['#5496d0'],
            height: "400",
            width: "100%",
            series: [

            ]
          }
        };
      }
    });
  }

  changeGraphFilter(event) {
    this.checkvaluequaility = false;
    let type = 'w';
    if (event.value === 'week') {
      type = 'w';
      this.qualityParamType = type;
    } else if (event.value === 'month') {
      type = 'm';
      this.qualityParamType = type;
    }
    if (this.textlabel) {
      var attnamedata = this.textlabel;
    } else {
      var attnamedata = this.attname[0].text
    }
    this.getStaticsGraph(type, attnamedata);


  }

  public isChartLoaded = false;

  getStaticsGraph(type, label) {
    this.isChartLoaded = false;
    if (this.sensId) {
      this.service.getQuilityGraph(this.sensId, type, label).subscribe(response => {
        let data = [];
        var hAxis= {
          title: "",
          slantedText:true,
          slantedTextAngle:45,
          titleTextStyle: {
            bold: true
          }
        }
        if (response.isSuccess === true) {
          if (response.data.length) {
            data.push(["Time", ""])
            response.data.forEach(element => {
              data.push([element.name, parseFloat(element.value)])
            });
          }
         
          this.columnChart3 = {
            chartType: "ColumnChart",
            dataTable: data,
            options: {
              title: "",
              vAxis: {
                title: "",
                titleTextStyle: {
                  bold: true
                },
                viewWindow: {
                  min: 0
                }
              },
              hAxis: hAxis,
              legend: { position: "none", alignment: "start" },
              bar: { groupWidth: "25%" },
              colors: ['#5496d0'],
              height: "400",
              width: "100%",
              series: [
  
              ]
            }
          };
        }
        setTimeout(() => {
          this.isChartLoaded = true;
        }, 200);
      });
    }
  }

  getAlertList() {
    this.alertsearchParameters.entityGuid = this.winguid;
    this.alertsearchParameters.deviceGuid = this.sensId;
    this.alertsService.getAlerts(this.alertsearchParameters).subscribe(response => {
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
      this.alerts = [];
      this._notificationService.add(new Notification('error', error));
    });
  }

  getDeviceName(wingId) {
    this.ishow = false;
    this.alerts = [];
    this.needleValue = 0;
    this.centralLabel = '';
    this.WidgetData = {};
    this.bottomLabel = ' ';
    this.options.arcColors = ['', ''];
    this.service.getSensorList(wingId).
      subscribe(response => {
        if (response.isSuccess === true && response.data != '' && response.data != null) {
          this.sensorList = response.data
          this.sensorList = this.sensorList.filter(word => word.isActive == true);
          this.sensguid = response.data[0].value;
          this.sensId = this.sensguid;
          this.sensguids = '';
          this.getsensorTelemetryData();
          this.getAlertList();
          //this.getStompConfig()
        } else {
          this.ishow = false;
          this.sensorList = [];
          if (response.message) {
            this._notificationService.add(new Notification('error', response.message));
          }
        }
      }, error => {
        this.spinner.hide();
        this._notificationService.add(new Notification('error', error));
      })
  }

  getSeUniqueName(sensId) {
    this.sensguid = sensId;
    this.ishow = false;
    this.checkvalue = true;
    this.checkvaluequaility = true;
    this.sensorId = sensId;
    this.alerts = [];
    this.needleValue = 0;
    this.selectedIndex = 3;
    this.centralLabel = '';
    this.WidgetData = {};
    this.bottomLabel = ' ';
    this.options.arcColors = ['', ''];
    this.sensId = sensId;
   
    this.getsensorLatestDetails(this.sensId);
   
    let obj = this.sensorList.find(o => o.value === sensId);
    this.getDeviceStatus(obj.text);
    this.senName = obj.text;
    if (this.senName) {
      let type = 'W'
      if (this.textlabel) {
        var attnamedata = this.textlabel;
      } else {
        var attnamedata = this.attname[0].text
      }
      
      this.getStaticsGraph(type, attnamedata);
      this.getCunsummerGraph(type, sensId);
      this.getsensorTelemetryData();
      this.getAlertList();
      this.getWQI(sensId);
      if (this.subscribed) {
        this.subscription.unsubscribe();
      }
      this.deviceIsConnected = false;
      this.messages = this.stompService.subscribe('/topic/' + this.cpId + '-' + this.senName);
      this.subscription = this.messages.subscribe(this.on_next);
      this.subscribed = true;
    }
  }

  getStompConfig() {

    this.deviceService.getStompConfig('LiveData').subscribe(response => {
      if (response.isSuccess) {
        this.stompConfiguration.url = response.data.url;
        this.stompConfiguration.headers.login = response.data.user;
        this.stompConfiguration.headers.passcode = response.data.password;
        this.stompConfiguration.headers.host = response.data.vhost;
        this.cpId = response.data.cpId;
        this.initStomp();
      }
    });
  }

  initStomp() {
    let config = this.stompConfiguration;
    this.stompService.config = config;
    this.stompService.initAndConnect();
    //this.stompSubscribe();
  }

  public stompSubscribe() {
    if (this.subscribed) {
      return;
    }

    this.messages = this.stompService.subscribe('/topic/' + this.cpId + '-' + this.senName);
    this.subscription = this.messages.subscribe(this.on_next);
    this.subscribed = true;
  }

  public on_next = (message: Message) => {
    let obj: any = JSON.parse(message.body);
    let reporting_data = obj.data.data.reporting
    this.isConnected = true;

    let dates = obj.data.data.time;
    let now = moment();
    //if (obj.data.data.status == 'off') {
    //  this.getsensorLatestDetails(this.sensorId)
    //}
    if (obj.data.data.status == undefined && obj.data.msgType == 'telemetry' && obj.data.msgType != 'device' && obj.data.msgType != 'simulator') {
      this.deviceIsConnected = true;
      this.WidgetData = reporting_data;

      this.optionsdata = {
        type: 'line',
        scales: {

          xAxes: [{
            type: 'realtime',
            time: {
              stepSize: 10
            },
            realtime: {
              duration: 90000,
              refresh: 7000,
              delay: 2000,
              onRefresh: function (chart: any) {
                if (chart.height) {
                  if (obj.data.data.status != 'on') {
                    chart.data.datasets.forEach(function (dataset: any) {

                      dataset.data.push({

                        x: now,

                        y: reporting_data[dataset.label]

                      });

                    });
                  }
                } else {

                }

              },
              // delay: 2000
            }
          }],
          yAxes: [{
            scaleLabel: {
              display: true,
              labelString: 'value'
            }
          }]
        },
        tooltips: {
          mode: 'nearest',
          intersect: false
        },
        hover: {
          mode: 'nearest',
          intersect: false
        }
      }
    } else if (obj.data.msgType === 'simulator' || obj.data.msgType === 'device') {
      if (obj.data.data.status === 'off') {
        this.deviceIsConnected = false;
      } else {
        this.deviceIsConnected = true;
      }
    }
    obj.data.data.time = now;
  }

  getsensorTelemetryData() {
    this.service.getsensorTelemetryData().subscribe(response => {
      if (response.isSuccess === true) {
        let temp = [];
        this.attname = response.data;
        let type = 'W';
        this.selectedIndex = 0
        response.data.forEach((element, i) => {
          var colorNames = Object.keys(this.chartColors);
          var colorName = colorNames[i % colorNames.length];
          var newColor = this.chartColors[colorName];
          var graphLabel = {
            label: element.text,
            backgroundColor: 'rgb(153, 102, 255)',
            borderColor: newColor,
            fill: false,
            cubicInterpolationMode: 'monotone',
            data: []
          }
          temp.push(graphLabel);
        });
        this.datasets = temp;
      } else {
        this.spinner.hide();
        if (response.message) {

          this._notificationService.add(new Notification('error', response.message));
        }
      }
    }, error => {
      this._notificationService.add(new Notification('error', error));
    });
  }

  createFormGroup() {
    this.wingForm = new FormGroup({
      parentEntityGuid: new FormControl(null),
      name: new FormControl('', [Validators.required]),
      description: new FormControl(''),
      isactive: new FormControl(''),
      imageFile: new FormControl(''),
    });
  }

  Respond() {
    this.fileName = '';
    this.fileUrl = null;
    this.wingForm.reset();
    this.fileToUpload = null;
    this.respondShow = true;
    this.isEdit = false;
    this.refresh();
  }

  closerepond() {
    this.fileName = '';
    this.fileUrl = null;
    this.fileToUpload = null;
    this.checkSubmitStatus = false;
    this.respondShow = false;
    this.wingObject.image = '';
    this.wingForm.reset();
    this.currentImage = null;
  }

  refresh() {
    this.createFormGroup();
    this.wingForm.reset(this.wingForm.value);
    this.wingModuleName = "Add Wing";
    this.wingGuid = null;
    this.buttonname = 'Add';
    this.isEdit = false;
    this.currentImage = null;
  }

  openDialog() {
    this.dialog.open(TelemetryPopupComponent, {
      width: '800px',
      height: 'auto'
    });
  }

  onTabChange() {
    this.columnChart = {
      chartType: 'ColumnChart',
      dataTable: [
        ['Day', 'Prediction', 'Actual Consumption'],
        ['Mon', 700, 1200],
        ['Tue', 300, 600],
        ['Web', 400, 500],
        ['Thu', 500, 1000],
        ['Fri', 600, 1100],
        ['Sat', 800, 1000],
        ['Sun', 100, 400]
      ],
      options: {
        bar: { groupWidth: "25%" },
        colors: ['#5496d0'],
        title: '',
        height: 250,
        // width:'100%',
        hAxis: {
          title: 'Day',
          gridlines: {
            count: 5
          },
        },
        vAxis: {
          title: 'Gallons',
          gridlines: {
            count: 1
          },
        }
      }
    };
  }

  getWingList(buildingGuid) {

    this.spinner.show();
    this.searchParameters['parentEntityGuid'] = buildingGuid;
    this.service.subentitylookup(this.buildingGuid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true && response.data != '' && response.data != null) {
        this.wingList = response.data;
        this.winguid = '';
        this.sensguids = '';
      }
      else {
        this.wingList = [];
      }
    }, error => {
      this.wingList = [];
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  getDeviceStatus(uniqueId) {
		this.service.getDeviceStatus(uniqueId).subscribe(response => {
      if (response.isSuccess === true) {
        this.deviceIsConnected = response.data.isConnected;
      } else {
				this._notificationService.add(new Notification('error', response.message));
			}
		}, error => {
			this._notificationService.add(new Notification('error', error));
		});
  }

  manageWing() {
    this.checkSubmitStatus = true;
    var data = {
      "parentEntityGuid": this.buildingGuid,
      "name": this.wingForm.value.name,
      "description": this.wingForm.value.description,
      "isactive": true,
      "countryGuid": this.buildingObj['countryGuid'],
      "stateGuid": this.buildingObj['stateGuid'],
      "city": this.buildingObj['city'],
      "zipcode": this.buildingObj['zipcode'],
      "address": this.buildingObj['address'],
      "latitude": this.buildingObj['latitude'],
      "longitude": this.buildingObj['longitude'],
    }
    if (this.isEdit) {
      if (this.wingGuid) {
        data["guid"] = this.wingGuid;
      }
      if (this.fileToUpload) {
        data["imageFile"] = this.fileToUpload;
      }
      data.isactive = this.wingObject['isactive']
    }
    else {
      data["imageFile"] = this.fileToUpload;
      this.wingForm.get('isactive').setValue(true);

    }
    if (this.wingForm.status === "VALID") {
      if (this.validstatus == true || !this.wingForm.value.imageFile) {
        this.spinner.show();
        this.service.addBuilding(data).subscribe(response => {
          this.spinner.hide();
           this.getWingList(this.buildingGuid);
           this.winguid = '';
           this.sensguids = '';
           this.sensorList = [];
           this.ishow = false;
          if (response.isSuccess === true) {
            if (this.isEdit) {
              this._notificationService.add(new Notification('success', "Wing has been updated successfully."));
              this.closerepond();
            } else {
              this._notificationService.add(new Notification('success', "Wing has been added successfully."));
              this.closerepond();
            }
            this.getWingList(this.buildingGuid);
          } else {
            this._notificationService.add(new Notification('error', response.message));
          }
        })
      } else {
        this.MesageAlertDataModel = {
          title: "Wing Image",
          message: "Invalid Image Type.",
          message2: "Upload .jpg, .jpeg, .png Image Only.",
          okButtonName: "OK",
        };
        const dialogRef = this.dialog.open(MessageDialogComponent, {
          width: '400px',
          height: 'auto',
          data: this.MesageAlertDataModel,
          disableClose: false
        });
      }
    }
  }

  getsensorDetails(senGuid) {
    this.spinner.show();
    this.service.getsensorDetails(senGuid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        let attrObj = {};
        response.data.forEach(element => {
          if (element.key) {
            attrObj[element.key] = element.value;
          }
        });
        this.WidgetData = attrObj;
      }
      else {
        this._notificationService.add(new Notification('error', response.message));

      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  getsensorLatestDetails(senGuid) {
    this.spinner.show();
    this.service.getsensorLatestDetails(senGuid).subscribe(response => {
      if (response.isSuccess === true) {
        let attrObj = {};
        response.data.forEach(element => {
          attrObj[element.attributeName] = element.attributeValue;
        });
        this.WidgetData = attrObj;
        this.ishow = true;
      }
      else {
        this.ishow = false;
        this._notificationService.add(new Notification('error', response.message));

      }
      this.spinner.hide();
    }, error => {
      this.ishow = false;
      this._notificationService.add(new Notification('error', error));
    });
  }

  getBuildingDetails(buildingGuid) {
    this.spinner.show();
    this.service.getbuildingDetails(buildingGuid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        this.buildingname = response.data.name;

      }
      else {
        this._notificationService.add(new Notification('error', response.message));

      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  getWingDetails(wingGuid) {
    this.closerepond();
    this.fileToUpload = false;
    this.wingObject.image = '';
    this.wingModuleName = "Edit Wing";
    this.wingGuid = wingGuid;
    this.isEdit = true;
    this.buttonname = 'Update';
    this.respondShow = true;
    this.spinner.show();
    this.service.getbuildingDetails(wingGuid).subscribe(response => {
      this.spinner.hide();
      this.buildingObj = response.data;
      if (response.isSuccess === true) {
        this.wingObject = response.data;
        if (this.wingObject.image) {
          this.wingObject.image = this.mediaUrl + this.wingObject.image;
          this.currentImage = this.wingObject.image;
        }
      }
      else {
        this._notificationService.add(new Notification('error', response.message));
        this.wingList = [];
      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
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
            that.wingObject.image = this.fileUrl
          }
        }
      } else {
        this.imageRemove();
        this.MesageAlertDataModel = {
          title: "Wing Image",
          message: "Invalid Image Type.",
          message2: "Upload .jpg, .jpeg, .png Image Only.",
          okButtonName: "OK",
        };
        const dialogRef = this.dialog.open(MessageDialogComponent, {
          width: '400px',
          height: 'auto',
          data: this.MesageAlertDataModel,
          disableClose: false
        });
      }
    }
  }

  deleteModel(wingModel: any) {
    this.deleteAlertDataModel = {
      title: "Delete Wing",
      message: this._appConstant.msgConfirm.replace('modulename', "Wing"),
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
        this.deleteWing(wingModel.guid);
      }
    });
  }

  deleteWing(guid) {
    this.spinner.show();
    this.service.deleteBuilding(guid).subscribe(response => {
      this.spinner.hide();
      if (response.isSuccess === true) {
        this.currentImage = '';
        this._notificationService.add(new Notification('success', this._appConstant.msgDeleted.replace("modulename", "Wing")));
        this.winguid = '';
        this.sensguids = '';
        this.sensorList = [];
        this.ishow = false;
        this.getWingList(this.buildingGuid);
      }
      else {
        this._notificationService.add(new Notification('error', response.message));
      }
    }, error => {
      this.spinner.hide();
      this._notificationService.add(new Notification('error', error));
    });
  }

  getWQI(deviceGuid) {
    this.deviceService.getwqiindexvalue(deviceGuid).subscribe(response => {
      if (response.isSuccess === true) {
        this.needleValue = response.data;
        this.centralLabel = response.data.toString();
        switch (response.data > 0) {
          case (this.needleValue >= 0 && this.needleValue <= 25):
            this.bottomLabel = 'Excellent';
            this.options.arcColors = ['#41c363', '#1774f0'];
            break;
          case (this.needleValue >= 26 && this.needleValue <= 50):
            this.bottomLabel = 'Good';
            this.options.arcColors = ['#1774f0', '#ffa800'];
            break;
          case (this.needleValue >= 51 && this.needleValue <= 75):
            this.bottomLabel = 'Poor';
            this.options.arcColors = ['#ffa800', '#e77800'];
            break;
          case (this.needleValue >= 76 && this.needleValue <= 100):
            this.bottomLabel = 'Very Poor';
            this.options.arcColors = ['#e77800', '#e55405'];
            break;
          case (this.needleValue > 100):
            this.bottomLabel = 'Unsuitable for drinking';
            break;
        }
      } else {
        this._notificationService.add(new Notification('error', response.message));
      }
    }, error => {
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

import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { CookieService } from 'ngx-cookie-service'
import { ApiConfigService, NotificationService } from 'app/services';

@Injectable()

export class DashboardService {
	cookieName = 'FM';
	protected apiServer = ApiConfigService.settings.apiServer;
	constructor(
		private http: HttpClient,
		private cookieService: CookieService,
		private _notificationService: NotificationService
	) { 
		this._notificationService.apiBaseUrl = this.apiServer.baseUrl;
	}

	getDashboardStatistics() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/statistics', configHeader).map(response => {
			return response;
		});
	}

	getNotificationList() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/notification', configHeader).map(response => {
			return response;
		});
	}

	getTruckUsage() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/getTruckUsage', configHeader).map(response => {
			return response;
		});
	}
	
	getTruckActivity() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/getTruckActivity', configHeader).map(response => {
			return response;
		});
	}

	getTruckGraph() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/getTruckGraph', configHeader).map(response => {
			return response;
		});
	}

	getStompCon() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get(this.apiServer.baseUrl + 'api/dashboard/getStompConfiguration', configHeader).map(response => {
			return response;
		});
	}
	
	getDeviceAttributeHistoricalData(data) {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.post(this.apiServer.baseUrl + 'api/dashboard/getDeviceAttributeHistoricalData', data, configHeader).map(response => {
			return response;
		});
	}

	getSensors(data) {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.post(this.apiServer.baseUrl + 'api/dashboard/getDeviceAttributes', data, configHeader).map(response => {
			return response;
		});
	}

	tripStatus(id,data) {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.put<any>(this.apiServer.baseUrl + 'api/trip/' + id +'/status', data, configHeader).map(response => {
			return response;
		});
	}

	startSimulator(id,isSalesTemplate = true) {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get<any>(this.apiServer.baseUrl + 'api/trip/startSimulator/'+id+'/'+isSalesTemplate, configHeader).map(response => {
			return response;
		});
	}

	getDashboardoverview() {
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get<any>(this.apiServer.baseUrl + 'api/dashboard/overview/' + '2D442AEA-E58B-4E8E-B09B-5602E1AA545A', configHeader).map(response => {
			return response;
		});
	}

	getLocationlist(parameters) {
		const parameter = {
			params: {
				'pageNo': parameters.pageNo + 1,
				'pageSize': parameters.pageSize,
				'searchText': parameters.searchText
			},
			timestamp: Date.now()
		};
		var configHeader = {
			headers: {
				'Content-Type': 'application/json',
				'Authorization': 'Bearer ' + this.cookieService.get(this.cookieName + 'access_token')
			}
		};

		return this.http.get<any>(this.apiServer.baseUrl + 'api/location/search', parameter).map(response => {
			return response;
		});
	}
	deleteLocation(locationGuid) {


		return this.http.put<any>(this.apiServer.baseUrl + 'api/location/delete/' + locationGuid, "").map(response => {
			return response;
		});
	}
}

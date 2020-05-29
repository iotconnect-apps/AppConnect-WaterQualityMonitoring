import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { CookieService } from 'ngx-cookie-service'
import { ApiConfigService, NotificationService } from 'app/services';
@Injectable({
  providedIn: 'root'
})
export class BuildingService {
  protected apiServer = ApiConfigService.settings.apiServer;
  cookieName = 'FM';
  constructor(private cookieService: CookieService,
    private httpClient: HttpClient,
    private _notificationService: NotificationService) { 
      this._notificationService.apiBaseUrl = this.apiServer.baseUrl;
    }

  getBuildingList(companyId) {
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/entity/' + companyId).map(response => {
      return response;
    });
  }
  
  getSensorList(wingId) {
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/devicelookup/' + wingId).map(response => {
      return response;
    });
  }
  getCunsummerGraph(type,sensguid) {
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/chart/getwaterconsumptionchartdata/' + sensguid + '/' +type).map(response => {
      return response;
    });
  }
  getQuilityGraph(sensguid,type,label) {
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/chart/deviceattributechartdata/' + sensguid + '/' + label+ '/' +type).map(response => {
      return response;
    });
  }
  getWingList(buildingGuid) {
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/entity/' + buildingGuid).map(response => {
      return response;
    });
  }
  getsensorTelemetryData() {
		return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/attributes').map(response => {
			return response;
		});
  }

  getBuilding(parameters) {
    const reqParameter = {
      params: {
        'parentEntityGuid': parameters.parentEntityGuid,
        'pageNo': parameters.pageNumber,
        'pageSize': parameters.pageSize,
        'searchText': parameters.searchText,
        'orderBy': parameters.sortBy
      },
      timestamp: Date.now()
    };
    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/entity/search', reqParameter).map(response => {
      return response;
    });
  }

  subentitylookup(entityId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/subentitylookup/' + entityId).map(response => {
      return response;
    });
  }

  getBuildingLookup(companyId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/entitylookup/' + companyId).map(response => {
      return response;
    });
  }

  removeBuildingImage(entityId) {
    return this.httpClient.put<any>(this.apiServer.baseUrl + 'api/entity/deleteimage/'+entityId,{}).map(response => {
      return response;
    });
  }

  
  getbuildingDetails(buildingGuid) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/entity/' + buildingGuid).map(response => {
      return response;
    });
  }
  getsensorDetails(sensGuid) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/getdeviceattributevalues/' + sensGuid).map(response => {
      return response;
    });
  }
  getsensorLatestDetails(sensGuid) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/telemetry/' + sensGuid).map(response => {
      return response;
    });
  }
  getBuidingOverview(buildingGuid) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/dashboard/getbuidingdetailoverview/' + buildingGuid).map(response => {
      return response;
    });
  }

  getElevatorLookup(buildingId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/getelevatorlookupbybuilding/' + buildingId).map(response => {
      return response;
    });
  }

  deleteBuilding(buildingGuid) {

    return this.httpClient.put<any>(this.apiServer.baseUrl + 'api/entity/delete/' + buildingGuid, "").map(response => {
      return response;
    });
  }

  getcountryList() {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/country').map(response => {
      return response;
    });
  }

  getcitylist(countryId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/lookup/state/' + countryId).map(response => {
      return response;
    });
  }

  
getDeviceStatus(uniqueId) {

  return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/connectionstatus/' + uniqueId).map(response => {
    return response;
  });
}
  addBuilding(data) {
    const formData = new FormData();
    for (const key of Object.keys(data)) {
      const value = data[key];
      if (data[key])
        formData.append(key, value);
    }

    return this.httpClient.post<any>(this.apiServer.baseUrl + 'api/entity/manage', formData).map(response => {
      return response;
    });
  }

  changeStatus(id, isActive) {
    let status = isActive == true ? false : true;
    return this.httpClient.post<any>(this.apiServer.baseUrl + 'api/entity/updatestatus/' + id + '/' + status, {}).map(response => {
      return response;
    });
  }

  getDeviceList(buildingId) {

    return this.httpClient.get<any>(this.apiServer.baseUrl + 'api/device/getbuildingdevices/' + buildingId).map(response => {
      return response;
    });
  }
}

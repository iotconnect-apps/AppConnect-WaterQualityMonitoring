import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'

import { SelectivePreloadingStrategy } from './selective-preloading-strategy'
import { PageNotFoundComponent } from './page-not-found.component'
import {
  HomeComponent, UserListComponent, UserAddComponent, DashboardComponent,
  LoginComponent, RegisterComponent, MyProfileComponent, ResetpasswordComponent, SettingsComponent,
  ChangePasswordComponent, AdminLoginComponent, SubscribersListComponent, HardwareListComponent, HardwareAddComponent, UserAdminListComponent, AdminUserAddComponent, AdminDashboardComponent, SubscriberDetailComponent, GeneratorAddComponent,
  BulkuploadAddComponent, GeneratorComponent, GeneratorListComponent, GeneratorDetailComponent,
  BuildingListComponent, AlertsComponent, BuildingDetailsComponent, BuildingAddComponent, RolesAddComponent, RolesListComponent, SensorListComponent, SensorAddComponent
} from './components/index'


import { AuthService, AdminAuthGuired } from './services/index'



const appRoutes: Routes = [
  {
    path: 'admin',
    children: [
      {
        path: '',
        component: AdminLoginComponent
      },
      {
        path: 'dashboard',
        component: AdminDashboardComponent,
        canActivate: [AuthService]
      },

      {
        path: 'subscribers/:email/:productCode/:companyId',
        component: SubscriberDetailComponent,
        canActivate: [AuthService]
      },
      {
        path: 'subscribers',
        component: SubscribersListComponent,
        canActivate: [AuthService]
      },
      {
        path: 'hardwarekits',
        component: HardwareListComponent,
        canActivate: [AuthService]
      },
      {
        path: 'hardwarekits/bulkupload',
        component: BulkuploadAddComponent,
        canActivate: [AuthService]
      },
      {
        path: 'hardwarekits/add',
        component: HardwareAddComponent,
        canActivate: [AuthService]
      },
      {
        path: 'hardwarekits/:hardwarekitGuid',
        component: HardwareAddComponent,
        canActivate: [AuthService]
      },
      {
        path: 'users',
        component: UserAdminListComponent,
        canActivate: [AuthService]
      },
      {
        path: 'users/add',
        component: AdminUserAddComponent,
        canActivate: [AuthService]
      },
      {
        path: 'users/:userGuid',
        component: AdminUserAddComponent,
        canActivate: [AuthService]
      }
    ]
  },
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  //App routes goes here
  {
    path: 'my-profile',
    component: MyProfileComponent,
    // canActivate: [AuthService]
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
    // canActivate: [AuthService]
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'buildings/:buildingGuid',
    component: BuildingAddComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'buildings/details/:buildingGuid',
    component: BuildingDetailsComponent,
    pathMatch: 'full'
    //canActivate: [AdminAuthGuired]
  },
  {
    path: 'buildings/add',
    component: BuildingAddComponent,
    //canActivate: [AdminAuthGuired]
  },
  {
    path: 'buildings',
    component: BuildingListComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'alerts',
    component: AlertsComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'alerts/:entityGuid/:deviceGuid',
    component: AlertsComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'building-details',
    component: BuildingDetailsComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'sensorkits',
    component: SensorListComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'sensorkits/add',
    component: SensorAddComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'sensorkits/:sensorGuid',
    component: SensorAddComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'generator',
    component: GeneratorComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'generator/add',
    component: GeneratorAddComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'generator/:generatorGuid',
    component: GeneratorAddComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'generatordetails/:generatordetailGuid',
    component: GeneratorDetailComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'roles/:deviceGuid',
    component: RolesAddComponent,
    canActivate: [AdminAuthGuired]
  }, {
    path: 'roles',
    component: RolesListComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'users/:userGuid',
    component: UserAddComponent,
    canActivate: [AdminAuthGuired]
  }, {
    path: 'users/add',
    component: UserAddComponent,
    canActivate: [AdminAuthGuired]
  }, {
    path: 'users',
    component: UserListComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: 'generators',
    component: GeneratorListComponent,
    canActivate: [AdminAuthGuired]
  },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes, {
      preloadingStrategy: SelectivePreloadingStrategy
    }
    )
  ],
  exports: [
    RouterModule
  ],
  providers: [
    SelectivePreloadingStrategy
  ]
})

export class AppRoutingModule { }

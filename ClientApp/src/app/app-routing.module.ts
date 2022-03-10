import { NgModule } from '@angular/core';
import { Routes, RouterModule, Router } from '@angular/router';  
import { GatewaysComponent } from './gateways/gateways.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { MasterGatewaysComponent } from './components/master-gateways/master-gateways.component';
import { AddGatewayComponent } from './components/add-gateway/add-gateway.component';
import { DetailGatewayComponent } from './components/detail-gateway/detail-gateway.component';
import { AddPeriphericalDeviceToGatewayComponent } from './components/add-peripherical-device-to-gateway/add-peripherical-device-to-gateway.component';
 
@NgModule({
  imports: [RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'master-gateways', component: MasterGatewaysComponent },
    { path: 'detail-gateway/:idGateway', component: DetailGatewayComponent },
    { path: 'add-gateway/:isEditedMode/:idGateway', component: AddGatewayComponent },
    { path: 'add-peripherical-device/:idGateway/:nameGateway', component: AddPeriphericalDeviceToGatewayComponent },
  ])],
  exports: [RouterModule]
})
export class AppRoutingModule { }

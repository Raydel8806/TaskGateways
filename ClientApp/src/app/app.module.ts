import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from "@angular/forms";
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { GatewaysComponent } from './gateways/gateways.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AppRoutingModule } from './app-routing.module';
import { MasterGatewaysComponent } from './components/master-gateways/master-gateways.component';
import { AddGatewayComponent } from './components/add-gateway/add-gateway.component';
import { DetailGatewayComponent } from './components/detail-gateway/detail-gateway.component';
import { AddPeriphericalDeviceToGatewayComponent } from './components/add-peripherical-device-to-gateway/add-peripherical-device-to-gateway.component';
import { GatewaysService } from './service/gateways.service';
import { DatePipe } from '@angular/common'; 

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    GatewaysComponent,
    AddGatewayComponent,
    HomeComponent, 
    FetchDataComponent,
    MasterGatewaysComponent,
    AddGatewayComponent,
    DetailGatewayComponent,
    AddPeriphericalDeviceToGatewayComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule,
    AppRoutingModule 
  ],
  providers: [GatewaysService, DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }

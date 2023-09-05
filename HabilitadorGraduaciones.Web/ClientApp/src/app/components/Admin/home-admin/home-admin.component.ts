import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SideNavService } from 'src/app/services/sideNav.service';

@Component({
  selector: 'app-home-admin',
  templateUrl: './home-admin.component.html',
  styleUrls: ['./home-admin.component.css']
})
export class HomeAdminComponent implements OnInit {

  constructor(public sideNavService : SideNavService, private router : Router, private cdref : ChangeDetectorRef) { }

  auth:boolean = true;
  ngOnInit() {
  }

  ngAfterViewChecked(): void {
    if(this.router.url == "/admin/unauthorized"){
      this.auth = false;
      this.cdref.detectChanges();
    }    
  }
}

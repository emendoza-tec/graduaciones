import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'app-roles-index',
  templateUrl: './roles-index.component.html',
  styleUrls: ['./roles-index.component.css']
})
export class RolesIndexComponent implements AfterViewInit {
  titulo: string = '';
  constructor(private titleService : TitleService, private cdRef: ChangeDetectorRef  ) {
  }

  ngAfterViewInit(): void {
    this.titleService.getTitle().subscribe(res => {
      this.titulo = res;
      this.cdRef.detectChanges(); 
    });
  }
}
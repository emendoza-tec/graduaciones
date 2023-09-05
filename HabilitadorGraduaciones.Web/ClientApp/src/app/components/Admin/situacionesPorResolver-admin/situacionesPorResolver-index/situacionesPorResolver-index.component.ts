import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { TitleService } from 'src/app/services/title.service';

@Component({
  selector: 'app-situacionesPorResolver-index',
  templateUrl: './situacionesPorResolver-index.component.html',
  styleUrls: ['./situacionesPorResolver-index.component.css']
})
export class SituacionesPorResolverIndexComponent implements AfterViewInit {
  titulo: string = '';
  constructor(private titleService : TitleService, private cdRef: ChangeDetectorRef  ) { }

  ngAfterViewInit(): void {
    this.titleService.getTitle().subscribe(res => {
      this.titulo = res;
      this.cdRef.detectChanges();
    });
  }
}

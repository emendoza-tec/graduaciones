import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BienvenidaGraduacionComponent } from './bienvenida-graduacion.component';

describe('BienvenidaGraduacionComponent', () => {
  let component: BienvenidaGraduacionComponent;
  let fixture: ComponentFixture<BienvenidaGraduacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BienvenidaGraduacionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BienvenidaGraduacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

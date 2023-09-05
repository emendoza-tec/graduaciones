import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroExitosoGraduacionComponent } from './registro-exitoso-graduacion.component';

describe('RegistroExitosoGraduacionComponent', () => {
  let component: RegistroExitosoGraduacionComponent;
  let fixture: ComponentFixture<RegistroExitosoGraduacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistroExitosoGraduacionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistroExitosoGraduacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

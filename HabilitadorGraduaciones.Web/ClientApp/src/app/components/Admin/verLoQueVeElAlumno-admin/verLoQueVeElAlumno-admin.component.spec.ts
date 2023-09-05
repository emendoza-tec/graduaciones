/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { VerLoQueVeElAlumnoAdminComponent } from './verLoQueVeElAlumno-admin.component';

describe('VerLoQueVeElAlumnoAdminComponent', () => {
  let component: VerLoQueVeElAlumnoAdminComponent;
  let fixture: ComponentFixture<VerLoQueVeElAlumnoAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerLoQueVeElAlumnoAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerLoQueVeElAlumnoAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

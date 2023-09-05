import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvisosAdminConfirmComponent } from './avisos-admin-confirm.component';

describe('AvisosAdminConfirmComponent', () => {
  let component: AvisosAdminConfirmComponent;
  let fixture: ComponentFixture<AvisosAdminConfirmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AvisosAdminConfirmComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AvisosAdminConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalDetalleNivelInglesComponent } from './modal-detalle-nivel-ingles.component';

describe('ModalDetalleNivelInglesComponent', () => {
  let component: ModalDetalleNivelInglesComponent;
  let fixture: ComponentFixture<ModalDetalleNivelInglesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalDetalleNivelInglesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalDetalleNivelInglesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

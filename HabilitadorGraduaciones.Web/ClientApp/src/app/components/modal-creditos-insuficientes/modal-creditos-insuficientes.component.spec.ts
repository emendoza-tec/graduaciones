import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreditosInsuficientesComponent } from './modal-creditos-insuficientes.component';

describe('ModalCreditosInsuficientesComponent', () => {
  let component: ModalCreditosInsuficientesComponent;
  let fixture: ComponentFixture<ModalCreditosInsuficientesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalCreditosInsuficientesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreditosInsuficientesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

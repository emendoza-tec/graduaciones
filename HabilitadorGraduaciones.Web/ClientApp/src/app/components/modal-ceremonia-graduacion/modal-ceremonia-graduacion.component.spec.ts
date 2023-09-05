import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCeremoniaGraduacionComponent } from './modal-ceremonia-graduacion.component';

describe('ModalCeremoniaGraduacionComponent', () => {
  let component: ModalCeremoniaGraduacionComponent;
  let fixture: ComponentFixture<ModalCeremoniaGraduacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalCeremoniaGraduacionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCeremoniaGraduacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

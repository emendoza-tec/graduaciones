import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModificarFechaGraduacionComponent } from './modificar-fecha-graduacion.component';

describe('ModificarFechaGraduacionComponent', () => {
  let component: ModificarFechaGraduacionComponent;
  let fixture: ComponentFixture<ModificarFechaGraduacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModificarFechaGraduacionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModificarFechaGraduacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

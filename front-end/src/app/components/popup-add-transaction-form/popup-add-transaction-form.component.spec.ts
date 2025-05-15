import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopupAddTransactionFormComponent } from './popup-add-transaction-form.component';

describe('PopupAddTransactionFormComponent', () => {
  let component: PopupAddTransactionFormComponent;
  let fixture: ComponentFixture<PopupAddTransactionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PopupAddTransactionFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopupAddTransactionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

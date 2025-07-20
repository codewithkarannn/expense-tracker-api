import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseIncomeLineGraphComponent } from './expense-income-line-graph.component';

describe('ExpenseIncomeLineGraphComponent', () => {
  let component: ExpenseIncomeLineGraphComponent;
  let fixture: ComponentFixture<ExpenseIncomeLineGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpenseIncomeLineGraphComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpenseIncomeLineGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { Component, ElementRef, inject, input, OnChanges, Signal, signal, SimpleChanges } from '@angular/core';
import { Chart, ChartConfiguration, ChartType } from 'chart.js';

@Component({
  selector: '[appChart]',
  imports: [],
  templateUrl: './expense-income-line-graph.component.html',
  styleUrl: './expense-income-line-graph.component.css'
})
export class ExpenseIncomeLineGraphComponent implements OnChanges {


  type =  input<ChartType>('line'); // Default type is 'line'
  data = input<any>();
  options = input<any>();
  chart = signal<any>(null);
  el =  inject(ElementRef);

  // OnChanges is a lifecycle hook that fires when any @Input property changes
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] && this.data()) {
      this.createOrUpdateChart();
    }
  }
    createOrUpdateChart() {

    // Create the new chart
    this.chart.set(new Chart(this.el.nativeElement, {
      type: this.type(),
      data: this.data(),
      options: this.options()
    }));  

    
  }

}

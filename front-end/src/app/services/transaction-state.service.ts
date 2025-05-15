import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionStateService {

  private readonly TAB_KEY = "active_tab";

  private activeTab$ = new BehaviorSubject<string>(this.getStoredTab());
  public readonly tab$ = this.activeTab$.asObservable();


  private refreshTrigger$ = new Subject<void>();
  public readonly onRefresh$ = this.refreshTrigger$.asObservable();


  setActiveTab(tab: string) {
    localStorage.setItem(this.TAB_KEY, tab);
    this.activeTab$.next(tab);
  }

  private getStoredTab(): string {
    return localStorage.getItem(this.TAB_KEY) || 'overview';
  }

  triggerRefresh() {
    this.refreshTrigger$.next();
    console.log("Refresh triggered");
  }
}

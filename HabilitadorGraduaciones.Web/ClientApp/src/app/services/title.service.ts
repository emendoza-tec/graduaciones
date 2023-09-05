import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class TitleService {
    private data = new BehaviorSubject<string>("");
    constructor() {
    }

    getTitle(): Observable<string> {
        return this.data.asObservable();
    }
    changeTitle(newTitle: string): void {
        this.data.next(newTitle)
    }
}
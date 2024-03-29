import { ApiResponse } from './../models/ApiResponse';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  _ApiUrl = `${environment.ApiRoot}`;
  constructor(public _HttpClient: HttpClient) { }
  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  UserLogin(Req: any): Observable<ApiResponse> {
    let Url = `${this._ApiUrl}/ClientSide/UserLogin`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  GetVerificationCode() {
    let Url = `${this._ApiUrl}/ClientSide/GetVerificationCode`;
    return this._HttpClient.post<ApiResponse>(Url, this._HttpOptions);
  }
  UserSignUp(Req: any) {
    let Url = `${this._ApiUrl}/ClientSide/UserSignUp`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  GetQuestions(Req: any) {
    let Url = `${this._ApiUrl}/Questions/Read`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  SaveUserAnswerArgs(Req: any) {
    let Url = `${this._ApiUrl}/ClientSide/UserAnswersUpdate`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  GetUserAnswer(Req: any) {
    let Url = `${this._ApiUrl}/AnswerQuestions/Read`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  GetUserData(Req:any){
    let Url = `${this._ApiUrl}/Users/Read`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  GetFiInfo(Req:any){
    let Url = `${this._ApiUrl}/ClientSide/ReadFiInfo`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
}

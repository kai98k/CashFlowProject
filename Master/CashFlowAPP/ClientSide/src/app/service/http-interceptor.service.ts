import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest,} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { GlobalToastService } from '../components/toast/global-toast.service';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {

  constructor(public _ToastService : GlobalToastService
  ) {

  }
  ShowToast(Msg:string,CssClass:string,Header:string) {
    this._ToastService.show(Msg,{
      className: CssClass,
      delay: 10000,
      HeaderTxt:Header,
    });
  }
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {


    let NewRequest;
    let Token = localStorage.getItem('Token');

    if (Token != 'null') {
      NewRequest = req.clone({
        setHeaders: {
          Authorization: 'Bearer ' + Token,
          AccessControlAllowOrigin: '*',
          AccessControlAllowMethods: 'GET, PUT, POST, DELETE, OPTIONS'
        }
      });
    } else {
      NewRequest = req;
    }

    return next.handle(NewRequest)
    .pipe(
      map((event: any) => {

        // https://www.tpisoftware.com/tpu/articleDetails/1084

        console.log('HttpInterceptorService event', event.body);
        if(event.body!==undefined||null){
          if(event.body.Success){
            this.ShowToast(event.body.Message,'bg-success text-light','成功通知 From 錢董')
          }
          else{
            this.ShowToast(event.body.Message,'bg-danger text-light','失敗通知 From 錢董')
          }
        }
        return event;
      }),
      catchError((error: HttpErrorResponse) => {

        console.log('HttpInterceptorService error', error);
        console.log('req', req);
        // https://stackoverflow.com/questions/68655492/throwerrorerror-is-now-deprecated-but-there-is-no-new-errorhttperrorresponse
        return throwError(() => error);

      }),
    );
  }
}


using System;
using System.Collections.Generic;

namespace SimulasiMBG
{
    
    public class Sekolah
    {
        public string idSekolah { get; set; }
        public string namaSekolah { get; set; }
        public int jumlahPaketDiterima { get; set; }

        public Sekolah(string idSekolah, string namaSekolah)
        {
            this.idSekolah = idSekolah;
            this.namaSekolah = namaSekolah;
            this.jumlahPaketDiterima = 0;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"[Sekolah] ID: {idSekolah} | Nama: {namaSekolah} | Total Paket Diterima: {jumlahPaketDiterima}");
        }
    }

    
    public abstract class PaketMBG
    {
        private string _idPaket;
        private string _namaPaket;
        private int _stokDapur;

        public string idPaket
        {
            get => _idPaket;
            set => _idPaket = value;
        }

        public string namaPaket
        {
            get => _namaPaket;
            set => _namaPaket = value;
        }

        public int stokDapur
        {
            get => _stokDapur;
            set
            {
                if (value >= 0)
                    _stokDapur = value;
                else
                    Console.WriteLine("Peringatan: Stok dapur tidak boleh negatif!");
            }
        }

        public PaketMBG(string idPaket, string namaPaket, int stokAwal)
        {
            this._idPaket = idPaket;
            this._namaPaket = namaPaket;
            this._stokDapur = stokAwal >= 0 ? stokAwal : 0;
        }

        
        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= _stokDapur)
            {
                _stokDapur -= jumlah;
            }
        }

        
        public abstract double HitungBiaya(int jumlah);
    }

    
    public class PaketReguler : PaketMBG
    {
        public PaketReguler(string idPaket, string namaPaket, int stokAwal)
            : base(idPaket, namaPaket, stokAwal) { }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 15000.0;
        }
    }

    public class PaketKhusus : PaketMBG
    {
        public string jenisDiet { get; set; }

        public PaketKhusus(string idPaket, string namaPaket, int stokAwal, string jenisDiet)
            : base(idPaket, namaPaket, stokAwal)
        {
            this.jenisDiet = jenisDiet;
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 20000.0;
        }
    }

    
    public interface IDapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiMBG : IDapatDidistribusikan
    {
        public Sekolah sekolahTujuan { get; set; }
        public PaketMBG paketKirim { get; set; }
        public int jumlahKirim { get; set; }

        public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
        {
            this.sekolahTujuan = sekolah;
            this.paketKirim = paket;
            this.jumlahKirim = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n--- Proses Distribusi ke {sekolahTujuan.namaSekolah} ---");
            if (jumlahKirim > 0 && jumlahKirim <= paketKirim.stokDapur)
            {
                paketKirim.KurangiStok(jumlahKirim);
                sekolahTujuan.jumlahPaketDiterima += jumlahKirim;

                double totalBiaya = paketKirim.HitungBiaya(jumlahKirim);

                Console.WriteLine($"[SUKSES] Distribusi MBG berhasil! Sebanyak {jumlahKirim} paket makanan ({paketKirim.namaPaket}) telah dikirim ke sekolah {sekolahTujuan.namaSekolah}.");
                Console.WriteLine($"Total Biaya Pengadaan: Rp {totalBiaya:N0}");
            }
            else
            {
                Console.WriteLine($"[GAGAL] Pengiriman ke {sekolahTujuan.namaSekolah} gagal!");
                if (jumlahKirim <= 0)
                {
                    Console.WriteLine("Alasan: Jumlah paket yang dikirim harus lebih dari 0.");
                }
                else
                {
                    Console.WriteLine($"Alasan: Jumlah kirim ({jumlahKirim}) melebihi stok dapur yang tersedia ({paketKirim.stokDapur}).");
                }
            }
        }
    }

    //Untuk menjalankan program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" SIMULASI PROGRAM MAKAN BERGIZI GRATIS (MBG) ");

            
            Sekolah sekolah1 = new Sekolah("SCH001", "SDN Jember Lor 1");
            Sekolah sekolah2 = new Sekolah("SCH002", "SMPN 1 Jember");

            
            PaketMBG paket1 = new PaketReguler("PKT01", "Paket Reguler Standar", 100);
            PaketMBG paket2 = new PaketKhusus("PKT02", "Paket Khusus Diet", 50, "Vegetarian");

            
            Console.WriteLine("\n[Status Awal Sekolah]");
            sekolah1.TampilInfo();
            sekolah2.TampilInfo();

            Console.WriteLine("\n[Status Awal Stok Dapur]");
            Console.WriteLine($"ID: {paket1.idPaket} | {paket1.namaPaket} | Stok Sisa: {paket1.stokDapur}");
            Console.WriteLine($"ID: {paket2.idPaket} | {paket2.namaPaket} | Jenis Diet: {(paket2 as PaketKhusus)?.jenisDiet} | Stok Sisa: {paket2.stokDapur}");

            
            DistribusiMBG distribusi1 = new DistribusiMBG(sekolah1, paket1, 30);
            distribusi1.KirimPaket();

            
            DistribusiMBG distribusi2 = new DistribusiMBG(sekolah2, paket2, 60);
            distribusi2.KirimPaket();

            
            DistribusiMBG distribusi3 = new DistribusiMBG(sekolah2, paket2, 20);
            distribusi3.KirimPaket();

            
          
            Console.WriteLine("[LAPORAN AKHIR SETELAH DISTRIBUSI]");
            sekolah1.TampilInfo();
            sekolah2.TampilInfo();
            Console.WriteLine($"Sisa Stok {paket1.namaPaket}: {paket1.stokDapur}");
            Console.WriteLine($"Sisa Stok {paket2.namaPaket}: {paket2.stokDapur}");
            

            
            Console.ReadKey();
        }
    }
}
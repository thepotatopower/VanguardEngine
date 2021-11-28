-- 忍竜 フシマチマドカ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Cont")
end

function Cont()
	local count = obj.GetNumberOf({q.Location, l.EnemyDrop})
	if count >= 4 then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
	if count >= 8 then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end
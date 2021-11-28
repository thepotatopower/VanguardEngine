-- グラビディア・スタンネル

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
	if obj.GetNumberOf({q.Location, l.Drop, q.Location, l.Order, q.Other, o.SetOrder}) >= 5 then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end
-- 深緑の守り人 パトリヤ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Cont")
end

function Cont()
	if obj.GetNumberOf({q.Location, l.PlayerUnits}) >= 5 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end
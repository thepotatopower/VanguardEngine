-- 樹角獣 エンピックス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
end

function Cont()
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
	if obj.GetNumberOf({q.Location, l.PlayerRC}) <= 3 then
		obj.AddCardState({q.Location, l.PlayerRC, q.Other, o.SameColumn}, cs.Resist, p.Continuous)
	end
end
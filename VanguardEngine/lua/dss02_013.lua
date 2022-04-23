-- 封焔竜 サムサーラ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
end

function Cont()
	if obj.IsPlayerTurn() and obj.GetNumberOf("Filter1", {l.VCArms}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
	if obj.GetNumberOf("Filter2", {l.VCArms}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Intercept, p.Continuous)
	end
end

function Filter1(id)
	return obj.IsOrderType(id, ot.RightDeityArms)
end

function Filter2(id)
	return obj.IsOrderType(id, ot.LeftDeityArms)
end
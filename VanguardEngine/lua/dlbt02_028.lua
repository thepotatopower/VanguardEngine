-- 晴天の恵光 ラーシエル

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.WhiteWings)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC, l.GC)
	ability1.SetActivation("Cont")
end

function Cont()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	else
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Intercept, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end
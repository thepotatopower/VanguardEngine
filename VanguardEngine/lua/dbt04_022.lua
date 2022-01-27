-- 柩機の竜 カルジャミード

function RegisterAbilities()
	-- cont rc
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("ContRC")
	-- cont gc
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.GC)
	ability2.SetActivation("ContGC")
end

function ContRC()
	if obj.IsDarkNight() or obj.IsAbyssalDarkNight() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
		obj.AddCardState({q.Other, o.This}, cs.CanInterceptFromBackRow, p.Continuous)
	end
end

function ContGC()
	if obj.IsAbyssalDarkNight() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusShield, 10000, p.Continuous)
	end
end
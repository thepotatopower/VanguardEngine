-- 樹角獣 ライナルバ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.SameColumn, q.Other, o.NotThis}, cs.BonusSkills, s.Boost, p.Continuous)
end
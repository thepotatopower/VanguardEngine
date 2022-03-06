-- 笑顔を共に ジーズヤ

function RegisterAbilities()
	-- cont1
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Cont1")
	-- cont2
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.GC)
	ability2.SetActivation("Cont2")
end

function Cont1()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Earnescorrect")}, cs.BonusShield, 10000, p.Continuous)
end

function Cont2()
	obj.AddCardState({q.Location, l.PlayerRC, q.NameContains, obj.LoadName("Earnescorrect")}, cs.CannotBeHit, p.Continuous)
end
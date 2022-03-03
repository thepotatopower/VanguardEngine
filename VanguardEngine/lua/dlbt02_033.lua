-- 魅了する美 エルティーヌ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC, l.RC)
	ability1.SetActivation("Cont1")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Powerful)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.VC)
	ability2.SetActivation("Cont2")
end

function Cont1()
	if obj.IsAttackingUnit() then
		obj.AddCardValue("Cont1Filter", {l.Units}, cs.BonusPower, 2000, p.Continuous)
	end
end

function Cont1Filter(id)
	return obj.IsThis(id)
end

function Cont2()
	if obj.SoulCount() == 0 then
		obj.AddPlayerString(ps.CanRideFromRideDeckWithoutDiscard, "dlbt02_002", p.Continuous)
	end
end
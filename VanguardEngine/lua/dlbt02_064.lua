-- 狙い撃つ美 レランジェ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC, l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Powerful)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.VC)
	ability2.SetActivation("Cont")
end

function ACTCost(check)
	if check then return obj.CanSB(1) and obj.CanDiscard(1) end
	obj.SoulBlast(1)
	obj.Discard(1)
end

function ACT()
	obj.Draw(1)
end

function Cont()
	if obj.SoulCount() == 0 then
		obj.AddPlayerString(ps.CanRideFromRideDeckWithoutDiscard, "dlbt02_033", p.Continuous)
	end
end
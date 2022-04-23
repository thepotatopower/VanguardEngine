-- 屍海の降霊術師 グレイヴ＝ゾルガ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCost("Cost")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.AlchemagicUsedThisTurn() then
		obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanDiscard(1) end
	obj.SoulBlast(1) 
	obj.Discard(1)
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.NormalOrder, q.Count, 1})
	obj.AddPlayerState(ps.CanAlchemagicDiff, p.UntilEndOfTurn)
end
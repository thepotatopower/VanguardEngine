-- ヘルヘイム・フェルベントレイジ

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function CanFullyResolve()
	return obj.InFinalRush()
end

function Activation()
	if obj.InFinalRush() then
		local called = obj.SuperiorCall({q.Location, l.Hand, q.Count, 2})
		obj.Store(called)
		obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end
-- 影纏い

function RegisterAbilities()
	-- on placed
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("OnPlacedTrigger")
	ability1.SetActivation("OnPlaced")
	-- on order played
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnOrderPlayed)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetTrigger("OnOrderTrigger")
	ability2.SetCanFullyResolve("OnOrderCanFullyResolve")
	ability2.SetCost("OnOrderCost")
	ability2.SetActivation("OnOrderPlayed")
end

function OnPlacedTrigger()
	return obj.IsApplicable()
end

function OnPlaced()
	local searched = obj.Select({q.Location, l.Deck, q.Other, o.Order, q.Count, 1, q.Min, 0})
	if obj.GetNumberOf({q.Location, l.Selected}) > 0 then
		obj.Reveal({q.Location, l.Selected})
		if not obj.Exists({q.Location, l.Drop, q.Name, obj.GetName({q.Location, l.Selected})}) then
			obj.AddToDrop({q.Location, l.Selected})
		end
	end
	obj.Shuffle()
end

function OnOrderTrigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Order, q.Other, o.Player})
end

function OnOrderCanFullyResolve()
	return obj.IsSameZone() or (obj.IsAlchemagic() and obj.Exists({q.Location, l.PlayerRC, q.Other, o.NotThisFieldID}))
end

function OnOrderCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnOrderPlayed()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.WasAlchemagic() then
		obj.ChooseAddToHand({q.Location, l.PlayerRC, q.Other, o.NotThisFieldID, q.Count, 1})
	end
end
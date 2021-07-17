-- Diaglass Sorceress

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 2, q.Min, 0
	elseif n == 4 then
		return q.Location, l.Looking, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRCFromHand() and (obj.VanguardIs("Hexaorb Sorceress") or obj.VanguardIs("Pentagleam Sorceress")) and obj.CanCB(1) and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.Discard(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(2)
		obj.Select(3)
		obj.SuperiorCall(5)
		if obj.Exists(4) then
			obj.RearrangeOnTop(4)
		end
	end
	return 0
end
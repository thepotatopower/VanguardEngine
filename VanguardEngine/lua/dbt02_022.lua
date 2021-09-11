-- Diaglass Sorceress

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking, q.Other, o.Unit, q.Count, 2, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRCFromHand() and (obj.VanguardIs("Hexaorb Sorceress") or obj.VanguardIs("Pentagleam Sorceress")) then
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

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(2)
		obj.Select(1)
		obj.SuperiorCall(3)
		if obj.Exists(2) then
			obj.RearrangeOnTop(3)
		end
	end
	return 0
end
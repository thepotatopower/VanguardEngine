-- Pentagleam Sorceress

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Looking
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Looking, q.Count, 1, q.Min, 0
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.PlacedOnRC, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Hexaorb Sorceress") then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(3)
		obj.RearrangeOnTop(1)
	elseif n == 2 then
		obj.LookAtTopOfDeck(1)
		obj.RearrangeOnTop(3)
		if obj.GetNumberOf(4) > 0 then
			obj.RearrangeOnTop(4)
		else
			obj.RearrangeOnBottom(1)
			obj.AddTempPower(2, 2000)
		end
	end
	return 0
end
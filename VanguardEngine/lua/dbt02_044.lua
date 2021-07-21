-- Exquisite Knight, Olwein

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 6
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 2, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Selected
	elseif n == 4 then
		return q.Location, l.Looking
	elseif n == 5 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 6 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	elseif n == 2 then
		return a.OnDriveCheck, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Hexaorb Sorceress") and obj.CanSB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.TriggerRevealed() and obj.CanCB(5) then
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

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	elseif n == 2 then
		obj.CounterBlast(5)
	end
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(2)
		obj.Select(2)
		obj.RearrangeOnTop(3)
		obj.RearrangeOnBottom(4)
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(6, 10000)
	end
	return 0
end
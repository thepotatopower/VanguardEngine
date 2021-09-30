-- Embodiment of Armor, Bahr

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Deck, q.Grade, 1, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.CB, 1
	elseif n == 2 then
		return a.OnAttackHits, p.IsMandatory, p.OncePerTurn
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Dragon Knight, Nehalem") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.VanguardIsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Store(obj.Search(1))
		obj.Reveal(obj.Stored())
		obj.Shuffle()
	elseif n == 2 then
		obj.AddTempPower(2, 5000)
	end
	return 0
end
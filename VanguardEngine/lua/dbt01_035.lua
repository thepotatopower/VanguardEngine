-- Protobulb Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.CB, 1, p.AddToSoul, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and (obj.IsAttackingUnit() or obj.IsBooster()) and obj.InFinalRush() and obj.Exists(1) then
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
		obj.ChooseAddToHand(2)
	end
	return 0
end